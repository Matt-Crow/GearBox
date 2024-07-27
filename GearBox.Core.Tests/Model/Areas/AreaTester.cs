using GearBox.Core.Model.Areas;
using Xunit;

namespace GearBox.Core.Tests.Model.Areas;

public class AreaTester
{
    [Fact]
    public void SpawnLootChest_GivenDefaultLootTable_DoesNotThrow()
    {
        var sut = new Area();
        sut.SpawnLootChest();
    }

    [Fact]
    public void SpawnEnemy_GivenDefaultEnemies_DoesNotThrow()
    {
        var sut = new Area();
        var actual = sut.SpawnEnemy();
        Assert.NotNull(actual);
    }
}