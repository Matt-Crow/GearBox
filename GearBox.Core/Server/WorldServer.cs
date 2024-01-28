namespace GearBox.Core.Server;

using System.Timers;
using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Stable;

public class WorldServer
{
    private readonly World _world;
    private readonly Dictionary<string, IConnection> _connections = new();
    private readonly Dictionary<string, Character> _players = new();
    private readonly Dictionary<string, CharacterController> _controls = new();
    private readonly Timer _timer;

    public WorldServer() : this(new World())
    {

    }

    public WorldServer(World world)
    {
        _world = world;
        
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
    public async Task AddConnection(string id, IConnection connection)
    {
        // might need to synchronize this
        if (!_connections.ContainsKey(id))
        {
            var character = new Character(); // will eventually read from repo
            var player = new PlayerCharacter(character);

            _world.AddStableObject(player);
            _world.AddDynamicObject(character);
            _connections.Add(id, connection);
            _players.Add(id, character);
            _controls.Add(id, new CharacterController(character));
            var payload = new WorldInit(
                character.Id, 
                _world.StaticContent.ToJson(),
                _world.ItemTypes.GetAll().Select(x => x.ToJson()).ToList()
            );
            var message = new Message<WorldInit>(MessageType.WorldInit, payload);
            await connection.Send(message);

            if (!_timer.Enabled)
            {
                _timer.Start();
            }
        }
    }

    public void RemoveConnection(string id)
    {
        if (!_connections.ContainsKey(id))
        {
            return;
        }

        var character = _players[id];
        if (character is not null)
        {
            _world.RemoveDynamicObject(character);
        }
        _players.Remove(id);
        _connections.Remove(id);
        _controls.Remove(id);

        if (!_connections.Any())
        {
            _timer.Stop();
        }
    }

    public CharacterController? GetControlsById(string id)
    {
        if (_controls.ContainsKey(id))
        {
            return _controls[id];
        }
        return null;
    }

    public async Task Update()
    {
        var stableChanges = _world.Update();

        // notify everyone of the update
        var body = new WorldUpdateJson(
            _world.DynamicContent.ToJson(),
            stableChanges.Select(c => c.ToJson()).ToList()
        );
        var message = new Message<WorldUpdateJson>(MessageType.WorldUpdate, body);
        var tasks = _connections.Values.Select(conn => conn.Send(message));
        await Task.WhenAll(tasks);
    }
}