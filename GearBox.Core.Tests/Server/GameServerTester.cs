using GearBox.Core.Model;
using GearBox.Core.Server;
using Xunit;

namespace GearBox.Core.Tests.Server;

public class GameServerTester
{
    [Fact]
    public async Task CannotConnectTwice()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame());

        await sut.AddConnection("foo", spy);
        await sut.AddConnection("foo", spy);

        Assert.Equal(1, sut.TotalConnections);
    }

    [Fact]
    public async Task AddThenRemoveBehaveAsExpected()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame());
        Assert.Equal(0, sut.TotalConnections);

        await sut.AddConnection("foo", spy);
        Assert.Equal(1, sut.TotalConnections);

        sut.RemoveConnection("foo");
        Assert.Equal(0, sut.TotalConnections);
    }

    [Fact]
    public async Task ClientReceivesAreaUponConnecting()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame());

        await sut.AddConnection("foo", spy);

        Assert.NotEmpty(spy.MessagesReceived);
    }

    [Fact]
    public async Task EachClientReceivesAreaUponUpdate()
    {
        var client1 = new SpyConnection();
        var client2 = new SpyConnection();
        var sut = new GameServer(MakeGame());
        await sut.AddConnection("foo", client1);
        await sut.AddConnection("bar", client2);

        await sut.Update();

        Assert.NotEmpty(client1.MessagesReceived);
        Assert.NotEmpty(client2.MessagesReceived);
    }

    [Fact]
    public async Task EnqueueCommand_GivenCommand_DoesNotExecuteImmediately()
    {
        var sut = new GameServer(MakeGame());
        await sut.AddConnection("foo", new SpyConnection());
        var command = new SpyControlCommand();

        sut.EnqueueCommand("foo", command);

        Assert.False(command.HasBeenExecuted);
    }

    [Fact]
    public async Task EnqueueCommand_GivenCommand_ExecutesAfterUpdate()
    {
        var sut = new GameServer(MakeGame());
        await sut.AddConnection("foo", new SpyConnection());
        var command = new SpyControlCommand();

        sut.EnqueueCommand("foo", command);
        await sut.Update();

        Assert.True(command.HasBeenExecuted);
    }

    public static IGame MakeGame()
    {
        var result = new GameBuilder()
            .WithArea("foo", 1, area => area.WithMap(new()))
            .Build();
        return result;
    }
}