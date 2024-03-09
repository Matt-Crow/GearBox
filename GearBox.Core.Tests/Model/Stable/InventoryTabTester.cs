using Xunit;

namespace GearBox.Core.Model.Stable;

public class InventoryTabTester
{
    [Fact]
    public void Add_GivenEmptyInventory_Works()
    {
        var sut = new InventoryTab();
        var expected = new Material(new ItemType("foo"));
        
        sut.Add(expected, 42);
        var actual = sut.Content.FirstOrDefault();

        Assert.Equal(expected, actual?.Item);
    }

    [Fact]
    public void Add_GivenStackableDuplicate_Sums()
    {
        var sut = new InventoryTab();
        var type = new ItemType("foo");
        
        sut.Add(new Material(type), 2);
        sut.Add(new Material(type), 3);
        var result = sut.Content.FirstOrDefault();

        Assert.Equal(type, result?.Item.Type);
        Assert.Equal(5, result?.Quantity);
    }

    [Fact]
    public void Add_GivenDifferent_DoesNotSum()
    {
        var sut = new InventoryTab();
        var item1 = new Material(new ItemType("foo"));
        var item2 = new Material(new ItemType("bar"));
        
        sut.Add(item1);
        sut.Add(item2);
        
        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0)?.Item);
        Assert.Equal(item2, result.ElementAtOrDefault(1)?.Item);
    }

    [Fact]
    public void Add_GivenEquipment_DoesNotSum()
    {
        var sut = new InventoryTab();
        var item1 = new Weapon(new ItemType("foo"));
        var item2 = new Weapon(new ItemType("foo"));
    
        sut.Add(item1);
        sut.Add(item2);

        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0)?.Item);
        Assert.Equal(item2, result.ElementAtOrDefault(1)?.Item);
    }
}