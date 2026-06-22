using GearBox.Core.Model.Items;
using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class LootTableTester
{
    [Fact]
    public void GetRandomItem_GivenMaterial_ReturnsIt()
    {
        var expected = new Material("foo");
        var sut = new LootTable([
            new LootOption(1, ItemUnion.OfMaterial(expected))
        ], new RandomNumberGenerator());

        var inventory = sut.GetRandomLoot();
        var actual = inventory.Materials.Content.First().Item;

        Assert.Equal(expected, actual);
        Assert.True(expected == actual);
    }

    [Fact]
    public void GetRandomItem_GivenPart_ReturnsCopyOfIt()
    {
        var expected = new Part("foo", PartSlotType.ALL.First());
        var sut = new LootTable([
            new LootOption(1, ItemUnion.OfPart(expected))
        ], new RandomNumberGenerator());

        var inventory = sut.GetRandomLoot();
        var actual = inventory
            .PartTabs
            .SelectMany(tab => tab.Content)
            .First()
            .Item;

        // IDs are different
        Assert.NotEqual(expected, actual);
        Assert.False(expected == actual);
    }
}