using GearBox.Core.Model;

namespace GearBox.Core.Server;

/// <summary>
/// A connection to a client.
/// See WorldServer for how implementors should handle receiving messages from
/// the connection.
/// </summary>
public interface IConnection
{
    /// <summary>
    /// Sends a message to the client.
    /// </summary>
    public Task Send<T>(Message<T> message) where T : IJson;
}