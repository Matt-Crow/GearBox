namespace GearBox.Core.Server;

using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Stable;
using System.Timers;

public class WorldServer
{
    private readonly World _world;
    private readonly Dictionary<string, IConnection> _connections = new();
    private readonly Dictionary<string, Character> _players = new();
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
        if (_connections.ContainsKey(id))
        {
            return;
        }

        var character = new Character(); // will eventually read from repo
        var player = new PlayerCharacter(character);

        // testing LootChests
        _world.AddTimer(new WorldTimer(() => 
        {
            _world.SpawnLootChest();
        }, 500));

        _world.StableContent.AddPlayer(player);
        _world.AddDynamicObject(character);
        _connections.Add(id, connection);
        _players.Add(id, character);
        var worldInit = new WorldInitJson(
            character.Id,
            _world.StaticContent.ToJson(),
            _world.ItemTypes.GetAll().Select(x => x.ToJson()).ToList()
        );
        await connection.Send(worldInit);

        // need to send all StableGameObjects to client
        var allStableObjects = _world.StableContent.GetAll()
            .Select(obj => Change.Created(obj))
            .Select(change => change.ToJson())
            .ToList();
        var worldUpdate = new WorldUpdateJson(
            _world.DynamicContent.ToJson(),
            allStableObjects
        );
        await connection.Send(worldUpdate);

        if (!_timer.Enabled)
        {
            _timer.Start();
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

        if (!_connections.Any())
        {
            _timer.Stop();
        }
    }

    /// <summary>
    /// Executes the given command on the player of the user with the given ID
    /// </summary>
    public void ExecuteCommand(string id, IControlCommand command)
    {
        if (!_players.ContainsKey(id))
        {
            throw new Exception($"Invalid id: \"{id}\"");
        }
        command.ExecuteOn(_players[id]);
    }

    public async Task Update()
    {
        var stableChanges = _world.Update();

        // notify everyone of the update
        var message = new WorldUpdateJson(
            _world.DynamicContent.ToJson(),
            stableChanges.Select(c => c.ToJson()).ToList()
        );
        var tasks = _connections.Values.Select(conn => conn.Send(message));
        await Task.WhenAll(tasks);
    }
}