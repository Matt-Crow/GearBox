namespace GearBox.Core.Tests.Server;

using GearBox.Core.Model;
using GearBox.Core.Server;

public class SpyConnection : IConnection
{
    private readonly List<Message<IJson>> _messagesReceived = new ();

    public IEnumerable<Message<IJson>> MessagesReceived { get => _messagesReceived.AsEnumerable(); }

    public Task Send<T>(Message<T> message) where T : IJson
    {
        // _messagesReceived.Add(message) fails: https://stackoverflow.com/a/10606974/11110116
        _messagesReceived.Add(new Message<IJson>(message.Type, message.Body));
        return Task.CompletedTask;
    }
}