using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class InventoryTabTester
{
    [Fact]
    public void Add_GivenEmptyInventory_Works()
    {
        var sut = new InventoryTab<Material>();
        var expected = new Material("foo");
        
        sut.Add(expected, 42);
        var actual = sut.Content.FirstOrDefault();

        Assert.Equal(expected, actual?.Item);
    }

    [Fact]
    public void Add_GivenStackableDuplicate_Sums()
    {
        var sut = new InventoryTab<Material>();

        sut.Add(new Material("foo"), 2);
        sut.Add(new Material("foo"), 3);
        var result = sut.Content.FirstOrDefault();

        Assert.Equal("foo", result?.Item.Name);
        Assert.Equal(5, result?.Quantity);
    }

    [Fact]
    public void Add_GivenDifferent_DoesNotSum()
    {
        var sut = new InventoryTab<Material>();
        var item1 = new Material("foo");
        var item2 = new Material("bar");
        
        sut.Add(item1);
        sut.Add(item2);
        
        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0)?.Item);
        Assert.Equal(item2, result.ElementAtOrDefault(1)?.Item);
    }

    [Fact]
    public void Add_GivenEquipment_DoesNotSum()
    {
        var sut = new InventoryTab<Equipment<WeaponStats>>();
        var item1 = new Equipment<WeaponStats>("foo", new WeaponStats());
        var item2 = new Equipment<WeaponStats>("foo", new WeaponStats());
    
        sut.Add(item1);
        sut.Add(item2);

        var result = sut.Content.ToList();
        Assert.Equal(item1, result.ElementAtOrDefault(0)?.Item);
        Assert.Equal(item2, result.ElementAtOrDefault(1)?.Item);
    }
}