using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Server;
using Xunit;

namespace GearBox.Core.Tests.Server;

public class GameServerTester
{
    public ConnectingUser AUser { get; set; } = new ConnectingUser("", "");

    [Fact]
    public async Task CannotConnectTwice()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());

        await sut.AddConnection("foo", AUser, spy);
        await sut.AddConnection("foo", AUser, spy);

        Assert.Equal(1, sut.TotalConnections);
    }

    [Fact]
    public async Task AddThenRemoveBehaveAsExpected()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());
        Assert.Equal(0, sut.TotalConnections);

        await sut.AddConnection("foo", AUser, spy);
        Assert.Equal(1, sut.TotalConnections);

        await sut.RemoveConnection("foo");
        Assert.Equal(0, sut.TotalConnections);
    }

    [Fact]
    public async Task ClientReceivesAreaUponConnecting()
    {
        var spy = new SpyConnection();
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());

        await sut.AddConnection("foo", AUser, spy);

        Assert.NotEmpty(spy.MessagesReceived);
    }

    [Fact]
    public async Task EachClientReceivesAreaUponUpdate()
    {
        var client1 = new SpyConnection();
        var client2 = new SpyConnection();
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());
        await sut.AddConnection("foo", AUser, client1);
        await sut.AddConnection("bar", AUser, client2);

        await sut.Update();

        Assert.NotEmpty(client1.MessagesReceived);
        Assert.NotEmpty(client2.MessagesReceived);
    }

    [Fact]
    public async Task EnqueueCommand_GivenCommand_DoesNotExecuteImmediately()
    {
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());
        await sut.AddConnection("foo", AUser, new SpyConnection());
        var command = new SpyControlCommand();

        sut.EnqueueCommand("foo", command);

        Assert.False(command.HasBeenExecuted);
    }

    [Fact]
    public async Task EnqueueCommand_GivenCommand_ExecutesAfterUpdate()
    {
        var sut = new GameServer(MakeGame(), MakePlayerCharacterRepository());
        await sut.AddConnection("foo", AUser, new SpyConnection());
        var command = new SpyControlCommand();

        sut.EnqueueCommand("foo", command);
        await sut.Update();

        Assert.True(command.HasBeenExecuted);
    }

    public static IGame MakeGame()
    {
        var result = new GameBuilder(new GearBoxConfig())
            .WithArea("foo", 1, area => area.WithMap(new()))
            .Build();
        return result;
    }

    public static IPlayerCharacterRepository MakePlayerCharacterRepository()
    {
        var result = new PlayerCharacterRepositoryMock();
        return result;
    }
}

class PlayerCharacterRepositoryMock : IPlayerCharacterRepository
{
    public Task<PlayerCharacter?> GetPlayerCharacterByAspNetUserIdAsync(string aspNetUserId)
    {
        return Task.FromResult<PlayerCharacter?>(null);
    }

    public Task SavePlayerCharacterAsync(PlayerCharacter playerCharacter, string aspNetUserId)
    {
        return Task.CompletedTask;
    }
}
