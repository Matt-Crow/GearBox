using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_DoesNothing()
    {
        var sut = new PlayerCharacter("foo", 1);
        sut.EquipWeaponById(Guid.Empty);
        Assert.Null(sut.WeaponSlot.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_SetsWeaponSlot()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(weapon.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        Assert.Equal(weapon, sut.WeaponSlot.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(weapon.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        Assert.False(sut.Inventory.Weapons.Contains(weapon));
    }

    [Fact]
    public void Equip_GivenWeaponAlreadyEquipped_AddsToInventory()
    {
        var sut = new PlayerCharacter("foo", 1);
        var alreadyEquipped = new Weapon(new ItemType("weapon 1"));
        var notYetEquipped = new Weapon(new ItemType("weapon 2"));
        sut.Inventory.Weapons.Add(alreadyEquipped);
        sut.Inventory.Weapons.Add(notYetEquipped);
        sut.EquipWeaponById(alreadyEquipped.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        sut.EquipWeaponById(notYetEquipped.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        Assert.True(sut.Inventory.Weapons.Contains(alreadyEquipped));
    }
}