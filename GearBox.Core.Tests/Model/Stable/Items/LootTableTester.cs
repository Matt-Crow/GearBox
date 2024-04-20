using GearBox.Core.Model.Stable.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable.Items;

public class LootTableTester
{
    [Fact]
    public void GetRandomItem_GivenNotItems_Throws()
    {
        var sut = new LootTable();
        Assert.Throws<InvalidOperationException>(() => sut.GetRandomItems());
    }

    [Fact]
    public void GetRandomItem_GivenSingle_ReturnsCopyOfIt()
    {
        var sut = new LootTable();
        var definition = new ItemDefinition<Material>(new ItemType("foo"), t => new Material(t));
        sut.AddMaterial(definition);

        var expected = definition.Create();
        var inventory = sut.GetRandomItems();
        var actual = inventory.Materials.Content.First().Item;

        Assert.Equal(expected, actual);
        Assert.False(expected == actual);
    }
}