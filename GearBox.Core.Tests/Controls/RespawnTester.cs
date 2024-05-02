using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic.Player;
using Xunit;

namespace GearBox.Core.Tests.Controls;

public class RespawnTester
{
    [Fact]
    public void ExecuteOn_GivenNotInWorld_Heals()
    {
        var sut = new Respawn();
        var world = new World();
        var player = new PlayerCharacter("foo", 1);
        world.SpawnPlayer(player);

        player.TakeDamage(999999);
        Assert.True(player.Termination.IsTerminated);

        world.Update(); // update world so it removes the player
        Assert.False(world.ContainsPlayer(player));

        sut.ExecuteOn(player, world);
        Assert.False(player.Termination.IsTerminated);
        Assert.True(world.ContainsPlayer(player));
    }

    [Fact]
    public void ExecuteOn_GivenAlreadyInWorld_DoesNotHeal()
    {
        var sut = new Respawn();
        var world = new World();
        var player = new PlayerCharacter("foo", 1);
        world.SpawnPlayer(player);
        player.TakeDamage(42);

        sut.ExecuteOn(player, world);

        Assert.Equal(42, player.DamageTaken);
    }
}