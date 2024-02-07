using GearBox.Core.Server;
using Xunit;

namespace GearBox.Core.Tests.Server;

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

        sut.RemoveConnection("foo");
        Assert.Equal(0, sut.TotalConnections);
    }

    [Fact]
    public async Task ClientReceivesWorldUponConnecting()
    {
        var spy = new SpyConnection();
        var sut = new WorldServer();

        await sut.AddConnection("foo", spy);

        Assert.NotEmpty(spy.MessagesReceived);
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

        Assert.NotEmpty(client1.MessagesReceived);
        Assert.NotEmpty(client2.MessagesReceived);
    }
}