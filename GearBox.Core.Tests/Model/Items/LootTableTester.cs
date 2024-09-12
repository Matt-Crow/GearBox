using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class LootTableTester
{
    [Fact]
    public void GetRandomItem_GivenMaterial_ReturnsIt()
    {
        var expected = new Material("foo");
        var sut = new LootTable([
            new LootOption(1, ItemUnion.Of(expected))
        ]);

        var inventory = sut.GetRandomLoot();
        var actual = inventory.Materials.Content.First().Item;

        Assert.Equal(expected, actual);
        Assert.True(expected == actual);
    }

    [Fact]
    public void GetRandomItem_GivenWeapon_ReturnsCopyOfIt()
    {
        var expected = new Equipment<WeaponStats>("foo", new WeaponStats());
        var sut = new LootTable([
            new LootOption(1, ItemUnion.Of(expected))
        ]);

        var inventory = sut.GetRandomLoot();
        var actual = inventory.Weapons.Content.First().Item;

        // IDs are different
        Assert.NotEqual(expected, actual);
        Assert.False(expected == actual);
    }
}