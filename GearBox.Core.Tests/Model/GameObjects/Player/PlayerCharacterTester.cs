using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Json;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_DoesNothing()
    {
        var sut = new PlayerCharacter("foo");
        sut.EquipWeaponById(Guid.Empty);
        Assert.Null(sut.Weapon);
    }

    [Fact]
    public void Equip_GivenWeapon_SetsWeaponSlot()
    {
        var sut = new PlayerCharacter("foo");
        var weapon = new Equipment<WeaponStats>("weapon", new WeaponStats());
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(GetId(weapon));

        Assert.Equal(weapon, sut.Weapon);
    }

    [Fact]
    public void Equip_GivenWeapon_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo");
        var weapon = new Equipment<WeaponStats>("weapon", new WeaponStats());
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(GetId(weapon));

        Assert.False(sut.Inventory.Weapons.Contains(weapon));
    }

    [Fact]
    public void Equip_GivenWeaponAlreadyEquipped_AddsToInventory()
    {
        var sut = new PlayerCharacter("foo");
        var alreadyEquipped = new Equipment<WeaponStats>("weapon 1", new WeaponStats());
        var notYetEquipped = new Equipment<WeaponStats>("weapon 2", new WeaponStats());
        sut.Inventory.Weapons.Add(alreadyEquipped);
        sut.Inventory.Weapons.Add(notYetEquipped);
        sut.EquipWeaponById(GetId(alreadyEquipped));

        sut.EquipWeaponById(GetId(notYetEquipped));

        Assert.True(sut.Inventory.Weapons.Contains(alreadyEquipped));
    }

    [Fact]
    public void CannotEquipWeaponsAboveOwnLevel()
    {
        var sut = new PlayerCharacter("foo");
        sut.SetLevel(1);
        var weapon = new Equipment<WeaponStats>("bar", new WeaponStats(), level: 20);
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(GetId(weapon));

        Assert.Null(sut.Weapon);
    }

    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var player = new PlayerCharacter("foo");
        var weapon1 = new Equipment<WeaponStats>("foo", new WeaponStats());
        player.Inventory.Weapons.Add(weapon1);
        var before = new UiState(player);
        
        player.EquipWeaponById(GetId(weapon1));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var player = new PlayerCharacter("foo");
        var weapon1 = new Equipment<WeaponStats>("foo", new WeaponStats());
        var weapon2 = new Equipment<WeaponStats>("foo", new WeaponStats()); // same weapon, different ID
        player.Inventory.Weapons.Add(weapon1);
        player.Inventory.Weapons.Add(weapon2);
        player.EquipWeaponById(GetId(weapon1));
        var before = new UiState(player);

        player.EquipWeaponById(GetId(weapon2));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }

    private Guid GetId<T>(Equipment<T> equipment)
    where T : IEquipmentStats
    {
        return equipment.Id ?? throw new Exception("ID should not be null");
    }
}