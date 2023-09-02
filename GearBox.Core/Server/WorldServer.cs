namespace GearBox.Core.Server;

using System.Timers;
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Static;

public class WorldServer
{
    private readonly World _world;
    private readonly HashSet<IConnection> _connections;
    private bool _running = false;
    private readonly Timer _timer;

    public WorldServer() : this(new World())
    {

    }

    public WorldServer(World world)
    {
        _world = world;
        _connections = new HashSet<IConnection>();
        
        // could use this instead, but read the comments 
        // https://stackoverflow.com/questions/75060940/how-to-use-game-loops-to-trigger-signalr-group-messages
        
        /*
        1000 ms   second
        ------- * -------
        second    frame
        */
        _timer = new Timer(1000.0 / Time.FRAMES_PER_SECOND)
        {
            AutoReset = true,
            Enabled = false
        };
        _timer.Elapsed += async (sender, e) => await Update();
    }

    public int TotalConnections { get => _connections.Count; }

    /// <summary>
    /// Adds the given connection, then sends the world
    /// </summary>
    public async Task AddConnection(IConnection connection)
    {
        // might need to synchronize this
        if (!_connections.Contains(connection))
        {
            _world.AddDynamicObject(new Character()); //todo attach controls
            _connections.Add(connection);
            var message = new Message<StaticWorldContentJson>(MessageType.WorldInit, _world.StaticContent.ToJson());
            await connection.Send(message);

            if (!_timer.Enabled)
            {
                _timer.Start();
            }
        }
    }

    public Task RemoveConnection(IConnection connection)
    {
        if (_connections.Contains(connection))
        {
            _connections.Remove(connection);
            //TODO maybe send message that they left

            if (!_connections.Any())
            {
                _timer.Stop();
            }
        }
        return Task.CompletedTask;
    }

    public async Task Update()
    {
        _world.Update();

        // notify everyone of the update
        var message = new Message<DynamicWorldContentJson>(MessageType.WorldUpdate, _world.DynamicContent.ToJson());
        var tasks = _connections.Select(conn => conn.Send(message));
        await Task.WhenAll(tasks);
    }
}