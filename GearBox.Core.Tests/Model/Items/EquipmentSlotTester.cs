using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class EquipmentSlotTester
{
    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var sut = new EquipmentSlot<Weapon>("");
        var tracker = new ChangeTracker(sut);
        
        sut.Value = new Weapon(new ItemType("foo"));
        
        Assert.True(tracker.HasChanged);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var sut = new EquipmentSlot<Weapon>("")
        {
            Value = new Weapon(new ItemType("foo"))
        };
        var tracker = new ChangeTracker(sut);

        sut.Value = new Weapon(new ItemType("foo"));

        Assert.True(tracker.HasChanged);
    }
}