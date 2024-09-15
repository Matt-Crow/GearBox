using GearBox.Core.Model.Areas;
using Xunit;

namespace GearBox.Core.Tests.Model.Areas;

public class AreaTester
{
    [Fact]
    public void SpawnLootChest_GivenDefaultLootTable_DoesNothing()
    {
        var sut = new Area();
        var actual = sut.SpawnLootChest();
        Assert.Null(actual);
    }

    [Fact]
    public void SpawnEnemy_GivenDefaultEnemies_DoesNothing()
    {
        var sut = new Area();
        var actual = sut.SpawnEnemy();
        Assert.Null(actual);
    }
}