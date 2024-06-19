using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Server;

public class WorldServer
{
    private readonly World _world;
    private readonly Dictionary<string, IConnection> _connections = [];
    private readonly Dictionary<string, PlayerCharacter> _players = [];
    private readonly List<PendingCommand> _pendingCommands = [];
    private readonly System.Timers.Timer _timer;
    private static readonly object connectionLock = new();

    public WorldServer() : this(new World())
    {

    }

    public WorldServer(World world)
    {
        _world = world;

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
    /// Adds the given connection, then sends the world
    /// </summary>
    public async Task AddConnection(string id, IConnection connection)
    {
        var task = Task.CompletedTask;
        lock (connectionLock)
        {
            task = DoAddConnection(id, connection);
        }
        await task;
    }

    private async Task DoAddConnection(string id, IConnection connection)
    {
        if (_connections.ContainsKey(id))
        {
            return;
        }

        var player = new PlayerCharacter("The Player"); // will eventually read from repo
        var spawnLocation = _world.Map.FindRandomFloorTile()
            ?? throw new Exception("Failed to find open tile. This should not happen.");
        player.Coordinates = spawnLocation.CenteredOnTile();

        _world.SpawnPlayer(player);
        _connections.Add(id, connection);
        _players.Add(id, player);

        // client needs to know both the world init and current state of stable objects
        await connection.Send(_world.GetWorldInitJsonFor(player));
        await connection.Send(_world.GetWorldUpdateJsonFor(player));

        if (!_timer.Enabled)
        {
            _timer.Start();
        }
    }

    public void RemoveConnection(string id)
    {
        lock (connectionLock)
        {
            DoRemoveConnection(id);
        }
    }

    private void DoRemoveConnection(string id)
    {
        if (!_connections.ContainsKey(id))
        {
            return;
        }

        _world.RemovePlayer(_players[id]);
        _players.Remove(id);
        _connections.Remove(id);

        if (!_connections.Any())
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
                command.Command.ExecuteOn(_players[command.ConnectionId], _world);
            }

            _world.Update();

            // notify everyone of the update
            tasks = _connections
                .Select(kv => kv.Value.Send(_world.GetWorldUpdateJsonFor(_players[kv.Key])))
                .ToList();
        }
        await Task.WhenAll(tasks);
    }
}