using GearBox.Core.Model.Json;
using GearBox.Core.Server;

namespace GearBox.Core.Tests.Server;

public class SpyConnection : IConnection
{
    private readonly List<IJson> _messagesReceived = new ();

    public IEnumerable<IJson> MessagesReceived => _messagesReceived.AsEnumerable();

    public Task Send<T>(string type, T message) where T : IJson
    {
        _messagesReceived.Add(message);
        return Task.CompletedTask;
    }
}