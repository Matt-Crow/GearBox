using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_Throws()
    {
        var sut = new PlayerCharacter("foo", 1);
        Assert.Throws<ArgumentException>(() => sut.EquipWeapon(new Weapon(new ItemType("bar"))));
    }

    [Fact]
    public void Equip_GivenWeapon_SetsWeaponSlot()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeapon(weapon);

        Assert.Equal(weapon, sut.Weapon.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeapon(weapon);

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
        sut.EquipWeapon(alreadyEquipped);

        sut.EquipWeapon(notYetEquipped);

        Assert.True(sut.Inventory.Weapons.Contains(alreadyEquipped));
    }
}