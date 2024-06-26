using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using Xunit;

namespace GearBox.Core.Tests.Controls;

public class RespawnTester
{
    [Fact]
    public void ExecuteOn_GivenNotInArea_Heals()
    {
        var area = new World();
        var sut = new Respawn(area);
        var player = new PlayerCharacter("foo");
        area.SpawnPlayer(player);

        player.TakeDamage(999999);
        Assert.True(player.Termination.IsTerminated);

        area.Update(); // update area so it removes the player
        Assert.Null(player.CurrentArea);

        sut.ExecuteOn(player);
        Assert.False(player.Termination.IsTerminated);
        Assert.Equal(area, player.CurrentArea);
    }

    [Fact]
    public void ExecuteOn_GivenAlreadyInArea_DoesNotHeal()
    {
        var area = new World();
        var sut = new Respawn(area);
        var player = new PlayerCharacter("foo");
        area.SpawnPlayer(player);
        player.TakeDamage(42);

        sut.ExecuteOn(player);

        Assert.Equal(42, player.DamageTaken);
    }
}