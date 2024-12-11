namespace GearBox.Core.Server;

public class ConnectionManager
{
    private readonly List<string> _pendingAdd = [];
    private readonly List<string> _pendingRemove = [];
    private readonly List<Func<Task>> _pendingCallbacks = [];
    private static readonly object connectionLock = new();

    /// <summary>
    /// Maps SignalR connection IDs to the player they control.
    /// </summary>
    public Dictionary<string, PlayerConnection> ConnectedPlayers { get; set; } = [];

    
    /// <summary>
    /// Enqueues a connection and a callback to run when RunCallbacks is invoked.
    /// </summary>
    public void EnqueueConnection(string connectionId, Func<Task> onConnect)
    {
        lock (connectionLock)
        {
            if (!IsConnected(connectionId))
            {
                _pendingAdd.Add(connectionId);
                _pendingCallbacks.Add(onConnect);
            }
        }
    }

    /// <summary>
    /// Enqueues a disconnection and a callback to run when RunCallbacks is invoked.
    /// </summary>
    public void EnqueueDisconnect(string connectionId, Func<Task> onDisconnect)
    {
        lock (connectionLock)
        {
            if (IsConnected(connectionId))
            {
                _pendingRemove.Add(connectionId);
                _pendingCallbacks.Add(onDisconnect);
            }
        }
    }

    private bool IsConnected(string connectionId)
    {
        var mightAdd = _pendingAdd.Contains(connectionId) || ConnectedPlayers.ContainsKey(connectionId);
        var result = mightAdd && !_pendingRemove.Contains(connectionId);
        return result;
    }

    /// <summary>
    /// Updates the connected players by running all registered callbacks.
    /// </summary>
    public async Task RunCallbacks()
    {
        var runThese = new List<Func<Task>>();
        lock (connectionLock)
        {
            runThese.AddRange(_pendingCallbacks);
            _pendingCallbacks.Clear();
            _pendingAdd.Clear();
        }
        await Task.WhenAll(runThese.Select(f => f()));
    }
}