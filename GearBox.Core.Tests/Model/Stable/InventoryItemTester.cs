using System.Text.Json;
using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class InventoryItemTester
{
    [Fact]
    public void Constructor_CannotAcceptNegativeQuantity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InventoryItem(new Stackable(), -1));
    }

    [Fact]
    public void Constructor_CanAcceptEmptyStack()
    {
        var stackable = new InventoryItem(new Stackable(), 0);
        Assert.Equal(0, stackable.Quantity);

        var nonStackable = new InventoryItem(new NonStackable(), 0);
        Assert.Equal(0, nonStackable.Quantity);
    }

    [Fact]
    public void Constructor_GivenNonStackable_CannotAcceptQuantityAbove1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InventoryItem(new NonStackable(), 2));
    }

    [Fact]
    public void AddQuantity_RejectsNegative()
    {
        var sut = new InventoryItem(new Stackable());
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(-1));
    }

    [Fact]
    public void AddQuantity_GivenNonStackableWithOne_RejectsPositive()
    {
        var sut = new InventoryItem(new NonStackable(), 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddQuantity(1));
    }

    private class Stackable : IInventoryItemType
    {
        public string ItemType => "stackable";

        public bool IsStackable => true;

        public string Serialize(JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    private class NonStackable : IInventoryItemType
    {
        public string ItemType => "nonStackable";

        public bool IsStackable => false;

        public string Serialize(JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}