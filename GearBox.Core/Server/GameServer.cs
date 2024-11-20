using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Server;

public class GameServer
{
    private readonly IGame _game;
    private readonly IPlayerCharacterRepository _playerCharacterRepository;
    private readonly Dictionary<string, IConnection> _connections = [];
    private readonly Dictionary<string, PlayerCharacter> _players = [];
    private readonly Dictionary<string, UiState?> _uiStates = [];
    private readonly Dictionary<string, ConnectingUser> _users = [];
    private readonly List<PendingCommand> _pendingCommands = [];
    private readonly System.Timers.Timer _timer;
    private static readonly object connectionLock = new();

    public GameServer(IGame game, IPlayerCharacterRepository playerCharacterRepository)
    {
        _game = game;
        _playerCharacterRepository = playerCharacterRepository;

        // could use this instead, but read the comments 
        // https://stackoverflow.com/questions/75060940/how-to-use-game-loops-to-trigger-signalr-group-messages
        
        // get the seconds in 1 frame, x1000 to get ms
        _timer = new System.Timers.Timer(Duration.FromFrames(1).InSeconds * 1000)
        {
            AutoReset = true,
            Enabled = false
        };
        _timer.Elapsed += async (sender, e) => await Update();
    }

    public int TotalConnections => _connections.Count;

    /// <summary>
    /// Adds the given connection, then sends the area
    /// </summary>
    public async Task AddConnection(string id, ConnectingUser user, IConnection connection)
    {
        var task = Task.CompletedTask;
        lock (connectionLock)
        {
            task = DoAddConnection(id, user, connection);
        }
        await task;
    }

    private async Task DoAddConnection(string id, ConnectingUser user, IConnection connection)
    {
        if (_connections.ContainsKey(id))
        {
            return;
        }

        // todo GetPlayerCharacterByUserId(user.Id)
        var player = new PlayerCharacter(user.Name); // will eventually read from repo
        var area = _game.GetDefaultArea();

        var spawnLocation = area.GetRandomFloorTile();
        player.Coordinates = spawnLocation.CenteredOnTile();

        area.SpawnPlayer(player);
        _connections.Add(id, connection);
        _players.Add(id, player);
        _uiStates.Add(id, null); // start with null UI state so it detects changes
        _users.Add(id, user);

        await SendGameInitTo(id);
        await SendAreaUpdateTo(id);

        if (!_timer.Enabled)
        {
            _timer.Start();
        }
    }

    private Task SendGameInitTo(string id)
    {
        var json = _game.GetGameInitJsonFor(_players[id]);
        return _connections[id].Send("GameInit", json);
    }

    public async Task RemoveConnection(string id)
    {
        var task = Task.CompletedTask;
        lock (connectionLock)
        {
            if (!_connections.ContainsKey(id))
            {
                return;
            }

            var player = _players[id];
            task = _playerCharacterRepository.SavePlayerCharacterAsync(player, _users[id].Id); 
            player.CurrentArea?.RemovePlayer(player);
            _players.Remove(id);
            _connections.Remove(id);
            _users.Remove(id);

            if (!_connections.Any())
            {
                _timer.Stop();
            }
        }
        await task;
    }

    /// <summary>
    /// Enqueues the given command so it will be executed during updates
    /// </summary>
    public void EnqueueCommand(string id, IControlCommand command)
    {
        _pendingCommands.Add(new(id, command));
    }

    public async Task Update()
    {
        var tasks = new List<Task>();
        lock (connectionLock)
        {
            var executeThese = _pendingCommands
                .Where(pc => _players.ContainsKey(pc.ConnectionId))
                .ToList();
            _pendingCommands.Clear();
            foreach (var command in executeThese)
            {
                try
                {
                    command.Command.ExecuteOn(_players[command.ConnectionId]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }

            _game.Update();
            tasks = _connections.Keys
                .Select(SendAreaUpdateTo)
                .ToList();
        }
        // notify everyone of the update
        await Task.WhenAll(tasks);
    }

    private Task SendAreaUpdateTo(string playerId)
    {
        var player = _players[playerId];
        var area = player.CurrentArea ?? player.LastArea; // last area in case they died
        if (area == null)
        {
            return Task.CompletedTask;
        }
        var json = area.GetAreaUpdateJsonFor(player);
        var newUiState = new UiState(player);
        var uiStateChanges = UiState.GetChanges(_uiStates[playerId], newUiState);
        _uiStates[playerId] = newUiState; // must come after computing changes
        json.UiStateChanges = uiStateChanges;
        return _connections[playerId].Send("AreaUpdate", json);
    }
}