using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class ItemTester
{
    [Fact]
    public void Constructor_CannotAcceptNegativeQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Item(ItemType.Stackable("foo"), -1));
    }

    [Fact]
    public void Constructor_CanAcceptEmptyStack()
    {
        var stackable = new Item(ItemType.Stackable("foo"), 0);
        Assert.Equal(0, stackable.Quantity);

        var nonStackable = new Item(ItemType.NonStackable("bar"), 0);
        Assert.Equal(0, nonStackable.Quantity);
    }

    [Fact]
    public void Constructor_GivenNonStackable_CannotAcceptQuantityAbove1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Item(ItemType.NonStackable("foo"), 2));
    }

    [Fact]
    public void AddQuantity_RejectsNegative()
    {
        var sut = new Item(ItemType.Stackable("foo"));
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(-1));
    }

    [Fact]
    public void AddQuantity_GivenNonStackableWithOne_RejectsPositive()
    {
        var sut = new Item(ItemType.NonStackable("foo"), 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(1));
    }
}