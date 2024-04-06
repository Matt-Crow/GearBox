using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Stable.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Dynamic.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_Throws()
    {
        var sut = new PlayerCharacter("foo", 1);
        Assert.Throws<ArgumentException>(() => sut.Equip(new Weapon(new ItemType("bar"))));
    }

    [Fact]
    public void Equip_GivenWeapon_SetsWeaponSlot()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Add(weapon);

        sut.Equip(weapon);

        Assert.Equal(weapon, sut.Weapon.Value);
    }

    [Fact]
    public void Equip_GivenWeapon_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo", 1);
        var weapon = new Weapon(new ItemType("weapon"));
        sut.Inventory.Add(weapon);

        sut.Equip(weapon);

        Assert.False(sut.Inventory.Contains(weapon));
    }

    [Fact]
    public void Equip_GivenWeaponAlreadyEquipped_AddsToInventory()
    {
        var sut = new PlayerCharacter("foo", 1);
        var alreadyEquipped = new Weapon(new ItemType("weapon 1"));
        var notYetEquipped = new Weapon(new ItemType("weapon 2"));
        sut.Inventory.Add(alreadyEquipped);
        sut.Inventory.Add(notYetEquipped);
        sut.Equip(alreadyEquipped);

        sut.Equip(notYetEquipped);

        Assert.True(sut.Inventory.Contains(alreadyEquipped));
    }
}