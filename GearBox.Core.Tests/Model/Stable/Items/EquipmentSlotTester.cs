using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Stable.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable.Items;

public class EquipmentSlotTester
{
    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var hasher = new DynamicHasher();
        var sut = new EquipmentSlot(Guid.Empty);
        var hashBefore = hasher.Hash(sut);
    
        sut.Value = new Weapon(new ItemType("foo"));
        var hashAfter = hasher.Hash(sut);

        Assert.NotEqual(hashBefore, hashAfter);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var hasher = new DynamicHasher();
        var sut = new EquipmentSlot(Guid.Empty)
        {
            Value = new Weapon(new ItemType("foo"))
        };
        var hashBefore = hasher.Hash(sut);

        sut.Value = new Weapon(new ItemType("foo"));
        var hashAfter = hasher.Hash(sut);

        Assert.NotEqual(hashBefore, hashAfter);
    }
}