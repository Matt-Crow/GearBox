using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Json;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class EquipmentSlotTester
{
    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var sut = new EquipmentSlot<WeaponStats>();
        var tracker = new ChangeTracker<ItemJson?>(sut);
        
        sut.Value = new Equipment<WeaponStats>(new ItemType("foo"), new WeaponStats());
        
        Assert.True(tracker.HasChanged);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var sut = new EquipmentSlot<WeaponStats>()
        {
            Value = new Equipment<WeaponStats>(new ItemType("foo"), new WeaponStats())
        };
        var tracker = new ChangeTracker<ItemJson?>(sut);

        sut.Value = new Equipment<WeaponStats>(new ItemType("foo"), new WeaponStats());

        Assert.True(tracker.HasChanged);
    }
}