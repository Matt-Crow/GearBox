namespace GearBox.Core.Server;

using GearBox.Core.Model;

public class WorldServer
{
    private readonly World _world;
    private readonly HashSet<IConnection> _connections;

    public WorldServer() : this(new World())
    {

    }

    public WorldServer(World world)
    {
        _world = world;
        _connections = new HashSet<IConnection>();
    }

    public int TotalConnections { get => _connections.Count; }

    /// <summary>
    /// Adds the given connection, then sends the world
    /// </summary>
    public async Task AddConnection(IConnection connection)
    {
        if (!_connections.Contains(connection))
        {
            _connections.Add(connection);
            var message = new Message(MessageType.WorldInit, _world.StaticContent);
            await connection.Send(message);
        }
    }

    public Task RemoveConnection(IConnection connection)
    {
        if (_connections.Contains(connection))
        {
            _connections.Remove(connection);
            //TODO maybe send message that they left
        }
        return Task.CompletedTask;
    }
}