using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class ItemStackTester
{
    [Fact]
    public void Constructor_CannotAcceptNegativeQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ItemStack<IItem>(new Material("foo"), -1));
    }

    [Fact]
    public void Constructor_CanAcceptEmptyStack()
    {
        var stackable = new ItemStack<IItem>(new Material("foo"), 0);
        Assert.Equal(0, stackable.Quantity);

        var nonStackable = new ItemStack<IItem>(new Material("bar"), 0);
        Assert.Equal(0, nonStackable.Quantity);
    }

    [Fact]
    public void AddQuantity_RejectsNegative()
    {
        var sut = new ItemStack<IItem>(new Material("foo"));
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(-1));
    }
}