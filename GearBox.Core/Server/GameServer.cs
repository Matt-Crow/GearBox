using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Server;

public class GameServer
{
    private readonly IGame _game;
    private readonly IPlayerCharacterRepository _playerCharacterRepository;
    private readonly ConnectionManager _connectionManager;
    private readonly List<PendingCommand> _pendingCommands = [];
    private readonly System.Timers.Timer _timer;


    public GameServer(IGame game, IPlayerCharacterRepository playerCharacterRepository)
    {
        _game = game;
        _playerCharacterRepository = playerCharacterRepository;
        _connectionManager = new ConnectionManager();

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
    public int TotalConnections => _connectionManager.ConnectedPlayers.Count;

    /// <summary>
    /// Enqueues the connection so it will start receiving messages starting on the next update.
    /// </summary>
    public void AddConnection(string id, ConnectingUser user, IConnection connection)
    {
        _connectionManager.EnqueueConnection(id, () => DoAddConnection(id, user, connection));
        if (!_timer.Enabled)
        {
            _timer.Start();
        }
    }

    private async Task DoAddConnection(string connectionId, ConnectingUser user, IConnection connection)
    {
        var player = await _playerCharacterRepository.GetPlayerCharacterByAspNetUserIdAsync(user.Id)
            ?? new PlayerCharacter(user.Name);
        
        var conn = new PlayerConnection(user.Id, connection, player);
        _connectionManager.ConnectedPlayers.Add(connectionId, conn);

        await conn.HandleConnect(_game);
    }

    /// <summary>
    /// Enqueues the connection for removal at the next update.
    /// </summary>
    public void RemoveConnection(string connectionId)
    {
        _connectionManager.EnqueueDisconnect(connectionId, () => DoDisconnect(connectionId));
    }

    private async Task DoDisconnect(string connectionId)
    {
        await _connectionManager.ConnectedPlayers[connectionId].HandleDisconnect(_playerCharacterRepository);
        _connectionManager.ConnectedPlayers.Remove(connectionId);

        // need to run this in here instead of in RemoveConnection so timer stops after the player has been saved to DB
        if (!_connectionManager.ConnectedPlayers.Any())
        {
            _timer.Stop();
        }
    }

    /// <summary>
    /// Enqueues the given command so it will be executed during updates
    /// </summary>
    public void EnqueueCommand(string id, IControlCommand command)
    {
        _pendingCommands.Add(new(id, command));
    }

    /// <summary>
    /// Runs all pending connection changes and commands, updates the game, then tells everyone about it.
    /// </summary>
    public async Task Update()
    {
        // check for pending connections
        await _connectionManager.RunCallbacks();

        // check for pending commands
        var executeThese = _pendingCommands
            .Where(pc => _connectionManager.ConnectedPlayers.ContainsKey(pc.ConnectionId))
            .ToList();
        _pendingCommands.Clear();
        foreach (var command in executeThese)
        {
            try
            {
                command.Command.ExecuteOn(_connectionManager.ConnectedPlayers[command.ConnectionId].Player);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        // update the game
        _game.Update();

        // notify everyone of the update
        var tasks = _connectionManager.ConnectedPlayers.Values
            .Select(cp => cp.SendAreaUpdate())
            .ToList();
        await Task.WhenAll(tasks);
    }
}