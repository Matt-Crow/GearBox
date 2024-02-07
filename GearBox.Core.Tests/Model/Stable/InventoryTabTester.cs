using Xunit;

namespace GearBox.Core.Model.Stable;

public class InventoryTabTester
{
    [Fact]
    public void Add_GivenEmptyInventory_Works()
    {
        var sut = new InventoryTab();
        var expected = new Item(ItemType.Stackable("foo"), 42);
        
        sut.Add(expected);
        var actual = sut.Content.FirstOrDefault();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_GivenStackableDuplicate_Sums()
    {
        var sut = new InventoryTab();
        var type = ItemType.Stackable("foo");
        
        sut.Add(new Item(type, 2));
        sut.Add(new Item(type, 3));
        var result = sut.Content.FirstOrDefault();

        Assert.Equal(type.Name, result?.ItemType.Name);
        Assert.Equal(5, result?.Quantity);
    }

    [Fact]
    public void Add_GivenNonStackableDuplicate_DoesNotSum()
    {
        var sut = new InventoryTab();
        var type = ItemType.NonStackable("foo");
        var item1 = new Item(type);
        var item2 = new Item(type);
        
        sut.Add(item1);
        sut.Add(item2);
        
        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0));
        Assert.Equal(item2, result.ElementAtOrDefault(1));
    }
}