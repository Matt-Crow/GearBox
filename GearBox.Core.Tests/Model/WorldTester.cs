using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Tests.Model.GameObjects;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class WorldTester
{
    [Fact]
    public void TwoWorldsWithSameIdAreEqual()
    {
        var world1 = new World();
        var world2 = new World(world1.Id);
        Assert.Equal(world1, world2);
    }

    [Fact]
    public void TwoWorldsWithDifferentIdAreNotEqual()
    {
        var world1 = new World();
        var world2 = new World();
        Assert.NotEqual(world1, world2);
    }

    [Fact]
    public void Update_GivenGameObjectsInWorld_ForwardsToThem()
    {
        var spy = new GameObjectSpy();
        var sut = new World();
        sut.GameObjects.AddGameObject(spy);
        sut.Update();
        Assert.True(spy.HasBeenUpdated);
    }

    [Fact]
    public void SpawnLootChest_GivenDefaultLootTable_Throws()
    {
        var sut = new World();

        Assert.Throws<InvalidOperationException>(sut.SpawnLootChest);
    }

    [Fact]
    public void SpawnEnemy_GivenDefaultEnemies_DoesNotThrow()
    {
        var sut = new World();
        var actual = sut.SpawnEnemy();
        Assert.NotNull(actual);
    }

    [Fact]
    public void Add_GivenSamePlayerTwice_OnlyAddsOnce()
    {
        var sut = new World();
        var player = new PlayerCharacter("foo", 1);

        sut.SpawnPlayer(player);
        sut.SpawnPlayer(player);
        sut.Update(); // apply changes

        Assert.Single(sut.GameObjects.GameObjects);
    }
}