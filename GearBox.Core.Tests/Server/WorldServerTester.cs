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

        await sut.AddConnection(spy);
        await sut.AddConnection(spy);

        Assert.Equal(1, sut.TotalConnections);
    }

    [Fact]
    public async Task AddThenRemoveBehaveAsExpected()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();
        Assert.Equal(0, sut.TotalConnections);

        await sut.AddConnection(spy);
        Assert.Equal(1, sut.TotalConnections);

        await sut.RemoveConnection(spy);
        Assert.Equal(0, sut.TotalConnections);
    }

    [Fact]
    public async Task ClientReceivesWorldUponConnecting()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();

        await sut.AddConnection(spy);

        Assert.Contains(spy.MessagesReceived, message => message.Type == MessageType.WorldInit);
    }
}