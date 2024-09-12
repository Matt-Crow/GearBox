using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_DoesNothing()
    {
        var sut = new PlayerCharacter("foo");
        sut.EquipWeaponById(Guid.Empty);
        Assert.Null(sut.WeaponSlot.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_SetsWeaponSlot()
    {
        var sut = new PlayerCharacter("foo");
        var weapon = new Equipment<WeaponStats>("weapon", new WeaponStats());
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(weapon.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        Assert.Equal(weapon, sut.WeaponSlot.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo");
        var weapon = new Equipment<WeaponStats>("weapon", new WeaponStats());
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(weapon.Id ?? throw new Exception("Weapon ID shouldn't be null"));

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
        sut.EquipWeaponById(alreadyEquipped.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        sut.EquipWeaponById(notYetEquipped.Id ?? throw new Exception("Weapon ID shouldn't be null"));

        Assert.True(sut.Inventory.Weapons.Contains(alreadyEquipped));
    }

    [Fact]
    public void CannotEquipWeaponsAboveOwnLevel()
    {
        var sut = new PlayerCharacter("foo");
        sut.SetLevel(1);
        var weapon = new Equipment<WeaponStats>("bar", new WeaponStats(), level: 20);
        sut.Inventory.Weapons.Add(weapon);

        sut.EquipWeaponById(weapon.Id ?? throw new Exception("Weapon ID should not be null"));

        Assert.Null(sut.WeaponSlot.Value);
    }
}