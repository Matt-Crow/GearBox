namespace GearBox.Core.Server;

/// <summary>
/// A connection to a client.
/// </summary>
public interface IConnection
{
    /// <summary>
    /// Sends a message to the client.
    /// </summary>
    public Task Send(Message message);

    /// <summary>
    /// Waits for a message to arrive from the client, then returns that message
    /// </summary>
    public Task<Message> Receive();
}