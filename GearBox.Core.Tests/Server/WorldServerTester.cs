namespace GearBox.Core.Tests.Server;

using GearBox.Core.Server;
using Xunit;

public class WorldServerTester
{
    [Fact]
    public async Task CannotConnectTwice()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();

        await sut.AddConnection("foo", spy);
        await sut.AddConnection("foo", spy);

        Assert.Equal(1, sut.TotalConnections);
    }

    [Fact]
    public async Task AddThenRemoveBehaveAsExpected()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();
        Assert.Equal(0, sut.TotalConnections);

        await sut.AddConnection("foo", spy);
        Assert.Equal(1, sut.TotalConnections);

        await sut.RemoveConnection("foo");
        Assert.Equal(0, sut.TotalConnections);
    }

    [Fact]
    public async Task ClientReceivesWorldUponConnecting()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();

        await sut.AddConnection("foo", spy);

        Assert.Contains(spy.MessagesReceived, message => message.Type == MessageType.WorldInit);
    }

    [Fact]
    public async Task EachClientReceivesWorldUponUpdate()
    {
        var client1 = new SpyConnection();
        var client2 = new SpyConnection();
        var sut = new WorldServer();
        await sut.AddConnection("foo", client1);
        await sut.AddConnection("bar", client2);

        await sut.Update();

        Assert.Contains(client1.MessagesReceived, message => message.Type == MessageType.WorldUpdate);
        Assert.Contains(client2.MessagesReceived, message => message.Type == MessageType.WorldUpdate);
    }
}