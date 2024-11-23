using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Server;

public class GameServer
{
    private readonly IGame _game;
    private readonly IPlayerCharacterRepository _playerCharacterRepository;
    private readonly Dictionary<string, PlayerConnection> _connectedPlayers = [];
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

    /// <summary>
    /// The total number of clients currently connected to the server.
    /// </summary>
    public int TotalConnections => _connectedPlayers.Count;

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

    private async Task DoAddConnection(string connectionId, ConnectingUser user, IConnection connection)
    {
        if (_connectedPlayers.ContainsKey(connectionId))
        {
            return;
        }

        var player = await _playerCharacterRepository.GetPlayerCharacterByAspNetUserIdAsync(user.Id)
            ?? new PlayerCharacter(user.Name);
        
        var conn = new PlayerConnection(user.Id, connection, player);
        _connectedPlayers.Add(connectionId, conn);

        await conn.HandleConnect(_game);

        if (!_timer.Enabled)
        {
            _timer.Start();
        }
    }

    public async Task RemoveConnection(string connectionId)
    {
        var task = Task.CompletedTask;
        lock (connectionLock)
        {
            if (!_connectedPlayers.ContainsKey(connectionId))
            {
                return;
            }

            task = _connectedPlayers[connectionId].HandleDisconnect(_playerCharacterRepository);
            _connectedPlayers.Remove(connectionId);

            if (!_connectedPlayers.Any())
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
                .Where(pc => _connectedPlayers.ContainsKey(pc.ConnectionId))
                .ToList();
            _pendingCommands.Clear();
            foreach (var command in executeThese)
            {
                try
                {
                    command.Command.ExecuteOn(_connectedPlayers[command.ConnectionId].Player);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }

            _game.Update();
            tasks = _connectedPlayers.Values
                .Select(cp => cp.SendAreaUpdate())
                .ToList();
        }
        // notify everyone of the update
        await Task.WhenAll(tasks);
    }
}