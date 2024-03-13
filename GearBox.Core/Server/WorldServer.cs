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
    private readonly Dictionary<string, PlayerCharacter> _players = new();
    private readonly Timer _timer;
    private static readonly object connectionLock = new();

    public WorldServer() : this(new World())
    {

    }

    public WorldServer(World world)
    {
        _world = world;

        // testing LootChests
        _world.AddTimer(new WorldTimer(() => _world.SpawnLootChest(), 50));

        
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

        var player = new PlayerCharacter(); // will eventually read from repo
        var spawnLocation = _world.StaticContent.Map.GetRandomOpenTile();
        if (spawnLocation != null)
        {
            player.Inner.Coordinates = spawnLocation.Value.CenteredOnTile();
        }

        _world.StableContent.AddPlayer(player);
        _world.DynamicContent.AddDynamicObject(player.Inner);
        _connections.Add(id, connection);
        _players.Add(id, player);
        var worldInit = new WorldInitJson(
            player.Inner.Id,
            _world.StaticContent.ToJson(),
            _world.ItemTypes.GetAll().Select(x => x.ToJson()).ToList()
        );
        await connection.Send(worldInit);

        // need to send all StableGameObjects to client
        var allStableObjects = _world.StableContent.GetAll()
            .Select(obj => Change.Content(obj))
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

        var player = _players[id];
        if (player is not null)
        {
            _world.DynamicContent.RemoveDynamicObject(player.Inner);
            _world.StableContent.RemovePlayer(player);
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
        var task = Task.CompletedTask;
        lock (connectionLock)
        {
            task = DoUpdate();
        }
        await task;
    }

    private async Task DoUpdate()
    {
        var stableChanges = _world.Update();
        // notify everyone of the update
        var message = new WorldUpdateJson(
            _world.DynamicContent.ToJson(),
            stableChanges.Select(c => c.ToJson()).ToList()
        );
        var tasks = _connections.Values
            .Select(conn => conn.Send(message))
            .ToList();
        await Task.WhenAll(tasks);
    }
}