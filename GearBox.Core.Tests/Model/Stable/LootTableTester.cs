using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class LootTableTester
{
    [Fact]
    public void GetRandomItem_GivenNotItems_Throws()
    {
        var sut = new LootTable();
        Assert.Throws<InvalidOperationException>(() => sut.GetRandomItem());
    }

    [Fact]
    public void GetRandomItem_GivenSingle_ReturnsCopyOfIt()
    {
        var sut = new LootTable();
        var definition = new ItemDefinition(new ItemType("foo"), t => new Material(t));
        sut.Add(definition);

        var expected = definition.Create();
        var actual = sut.GetRandomItem();

        Assert.Equal(expected, actual);
        Assert.False(expected == actual);
    }
}