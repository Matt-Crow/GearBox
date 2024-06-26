using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Server;

public class AreaServer
{
    private readonly Dictionary<string, IConnection> _connections = [];
    private readonly Dictionary<string, PlayerCharacter> _players = [];
    private readonly List<PendingCommand> _pendingCommands = [];
    private readonly System.Timers.Timer _timer;
    private static readonly object connectionLock = new();

    public AreaServer(IArea? area = null)
    {
        Area = area ?? new World();

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

    public IArea Area { get; init; }
    public int TotalConnections => _connections.Count;

    /// <summary>
    /// Adds the given connection, then sends the area
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
        var spawnLocation = Area.GetRandomFloorTile();
        player.Coordinates = spawnLocation.CenteredOnTile();

        Area.SpawnPlayer(player);
        _connections.Add(id, connection);
        _players.Add(id, player);

        // client needs to know both the area init and current state of stable objects
        await SendAreaInitTo(id);
        await SendAreaUpdateTo(id);

        if (!_timer.Enabled)
        {
            _timer.Start();
        }
    }

    private Task SendAreaInitTo(string id)
    {
        var json = Area.GetAreaInitJsonFor(_players[id]);
        return _connections[id].Send("AreaInit", json);
    }

    public void RemoveConnection(string id)
    {
        lock (connectionLock)
        {
            if (!_connections.ContainsKey(id))
            {
                return;
            }

            Area.RemovePlayer(_players[id]);
            _players.Remove(id);
            _connections.Remove(id);

            if (!_connections.Any())
            {
                _timer.Stop();
            }
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
        lock (connectionLock)
        {
            var executeThese = _pendingCommands
                .Where(pc => _players.ContainsKey(pc.ConnectionId))
                .ToList();
            _pendingCommands.Clear();
            foreach (var command in executeThese)
            {
                command.Command.ExecuteOn(_players[command.ConnectionId]);
            }

            Area.Update();
        }
        // notify everyone of the update
        await Task.WhenAll(_connections.Keys.Select(SendAreaUpdateTo));
    }

    private Task SendAreaUpdateTo(string playerId)
    {
        var json = Area.GetAreaUpdateJsonFor(_players[playerId]);
        return _connections[playerId].Send("AreaUpdate", json);
    }
}