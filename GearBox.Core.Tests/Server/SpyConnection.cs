namespace GearBox.Core.Tests.Server;

using GearBox.Core.Server;

public class SpyConnection : IConnection
{
    private readonly List<Message> _messagesReceived = new ();

    public IEnumerable<Message> MessagesReceived { get => _messagesReceived.AsEnumerable(); }

    public Task Send(Message message)
    {
        _messagesReceived.Add(message);
        return Task.CompletedTask;
    }

    public Task<Message> Receive()
    {
        throw new Exception();
    }
}