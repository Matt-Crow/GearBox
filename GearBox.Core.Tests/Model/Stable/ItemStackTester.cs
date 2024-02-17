using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class ItemStackTester
{
    [Fact]
    public void Constructor_CannotAcceptNegativeQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ItemStack(new Material(new ItemType("foo")), -1));
    }

    [Fact]
    public void Constructor_CanAcceptEmptyStack()
    {
        var stackable = new ItemStack(new Material(new ItemType("foo")), 0);
        Assert.Equal(0, stackable.Quantity);

        var nonStackable = new ItemStack(new Material(new ItemType("bar")), 0);
        Assert.Equal(0, nonStackable.Quantity);
    }

    [Fact]
    public void AddQuantity_RejectsNegative()
    {
        var sut = new ItemStack(new Material(new ItemType("foo")));
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(-1));
    }
}