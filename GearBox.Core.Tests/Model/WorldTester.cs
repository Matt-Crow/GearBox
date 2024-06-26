using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Tests.Model.GameObjects;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class WorldTester
{
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
}