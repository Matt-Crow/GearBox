using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Json;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class EquipmentSlotTester
{
    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var player = new PlayerCharacter("foo");
        var before = new UiState(player);
        
        player.WeaponSlot.Value = new Equipment<WeaponStats>("foo", new WeaponStats());
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var player = new PlayerCharacter("foo");
        player.WeaponSlot.Value = new Equipment<WeaponStats>("foo", new WeaponStats());
        var before = new UiState(player);

        player.WeaponSlot.Value = new Equipment<WeaponStats>("foo", new WeaponStats());
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }
}