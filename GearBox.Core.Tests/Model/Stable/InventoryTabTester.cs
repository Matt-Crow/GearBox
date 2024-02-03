using Xunit;

namespace GearBox.Core.Model.Stable;

public class InventoryTabTester
{
    [Fact]
    public void Add_GivenEmptyInventory_Works()
    {
        var sut = new InventoryTab();
        var expected = new InventoryItem(InventoryItemType.Stackable("foo"), 42);
        
        sut.Add(expected);
        var actual = sut.Content.FirstOrDefault();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_GivenStackableDuplicate_Sums()
    {
        var sut = new InventoryTab();
        var type = InventoryItemType.Stackable("foo");
        
        sut.Add(new InventoryItem(type, 2));
        sut.Add(new InventoryItem(type, 3));
        var result = sut.Content.FirstOrDefault();

        Assert.Equal(type.Name, result?.ItemType.Name);
        Assert.Equal(5, result?.Quantity);
    }

    [Fact]
    public void Add_GivenNonStackableDuplicate_DoesNotSum()
    {
        var sut = new InventoryTab();
        var type = InventoryItemType.NonStackable("foo");
        var item1 = new InventoryItem(type);
        var item2 = new InventoryItem(type);
        
        sut.Add(item1);
        sut.Add(item2);
        
        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0));
        Assert.Equal(item2, result.ElementAtOrDefault(1));
    }
}