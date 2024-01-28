using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class InventoryItemTester
{
    [Fact]
    public void Constructor_CannotAcceptNegativeQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InventoryItem(InventoryItemType.Stackable("foo"), -1));
    }

    [Fact]
    public void Constructor_CanAcceptEmptyStack()
    {
        var stackable = new InventoryItem(InventoryItemType.Stackable("foo"), 0);
        Assert.Equal(0, stackable.Quantity);

        var nonStackable = new InventoryItem(InventoryItemType.NonStackable("bar"), 0);
        Assert.Equal(0, nonStackable.Quantity);
    }

    [Fact]
    public void Constructor_GivenNonStackable_CannotAcceptQuantityAbove1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InventoryItem(InventoryItemType.NonStackable("foo"), 2));
    }

    [Fact]
    public void AddQuantity_RejectsNegative()
    {
        var sut = new InventoryItem(InventoryItemType.Stackable("foo"));
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(-1));
    }

    [Fact]
    public void AddQuantity_GivenNonStackableWithOne_RejectsPositive()
    {
        var sut = new InventoryItem(InventoryItemType.NonStackable("foo"), 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(1));
    }
}