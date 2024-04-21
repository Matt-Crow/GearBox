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
    public void GetRandomItem_GivenMaterial_ReturnsIt()
    {
        var sut = new LootTable();
        var expected = new Material(new ItemType("foo"));
        sut.AddMaterial(expected);

        var inventory = sut.GetRandomItems();
        var actual = inventory.Materials.Content.First().Item;

        Assert.Equal(expected, actual);
        Assert.True(expected == actual);
    }

    [Fact]
    public void GetRandomItem_GivenWeapon_ReturnsCopyOfIt()
    {
        var sut = new LootTable();
        var expected = new Weapon(new ItemType("foo"));
        sut.AddEquipment(expected);

        var inventory = sut.GetRandomItems();
        var actual = inventory.Equipment.Content.First().Item;

        // IDs are different
        Assert.NotEqual(expected, actual);
        Assert.False(expected == actual);
    }
}