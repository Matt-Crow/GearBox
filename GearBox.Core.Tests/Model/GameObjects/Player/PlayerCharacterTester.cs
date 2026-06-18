using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Json.AreaUpdate;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Player;

public class PlayerCharacterTester
{
    [Fact]
    public void Equip_GivenNotInInventory_DoesNothing()
    {
        var sut = new PlayerCharacter("foo");
        sut.EquipById(Guid.Empty);
        Assert.Empty(sut.EquipmentSlots.Where(slot => slot.Equipment != null));
    }

    [Fact]
    public void Equip_GivenEquipment_SetsAppropriateSlot()
    {
        var sut = new PlayerCharacter("foo");
        var equipment = AnEquipment();
        sut.Inventory.Add(equipment);

        sut.EquipById(GetId(equipment));

        Assert.Equal(equipment, sut.GetSlotFor(equipment).Equipment);
    }

    [Fact]
    public void Equip_GivenEquipment_RemovesFromInventory()
    {
        var sut = new PlayerCharacter("foo");
        var equipment = AnEquipment();
        sut.Inventory.Add(equipment);

        sut.EquipById(GetId(equipment));

        Assert.False(sut.Inventory.Contains(ItemUnion.OfEquipment(equipment)));
    }

    [Fact]
    public void Equip_GivenSlotAlreadyEquipped_AddsToInventory()
    {
        var sut = new PlayerCharacter("foo");
        var alreadyEquipped = new Equipment("Equipment 1", EquipmentSlotType.WEAPON);
        var notYetEquipped = new Equipment("Equipment 2", EquipmentSlotType.WEAPON);
        sut.Inventory.Add(alreadyEquipped);
        sut.Inventory.Add(notYetEquipped);
        sut.EquipById(GetId(alreadyEquipped));

        sut.EquipById(GetId(notYetEquipped));

        Assert.True(sut.Inventory.Contains(ItemUnion.OfEquipment(alreadyEquipped)));
    }

    [Fact]
    public void CannotEquipAboveOwnLevel()
    {
        var sut = new PlayerCharacter("foo");
        sut.SetLevel(1);
        var equipment = new Equipment("bar", EquipmentSlotType.WEAPON, level: 20);
        sut.Inventory.Add(equipment);

        sut.EquipById(GetId(equipment));

        Assert.Null(sut.GetSlotFor(equipment).Equipment);
    }

    [Fact]
    public void DynamicValues_AfterEquipping_Change()
    {
        var player = new PlayerCharacter("foo");
        var equipment = AnEquipment();
        player.Inventory.Add(equipment);
        var before = new UiState(player);
        
        player.EquipById(GetId(equipment));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }

    [Fact]
    public void DynamicValues_AfterSwitching_Change()
    {
        var player = new PlayerCharacter("foo");
        var equipment1 = AnEquipment();
        var equipment2 = AnEquipment(); // same equipment, different ID
        player.Inventory.Add(equipment1);
        player.Inventory.Add(equipment2);
        player.EquipById(GetId(equipment1));
        var before = new UiState(player);

        player.EquipById(GetId(equipment2));
        var after = new UiState(player);
        var compared = UiState.GetChanges(before, after);
        
        Assert.True(compared.Weapon.HasChanged);
    }

    private Guid GetId(Equipment equipment)
    {
        return equipment.Id ?? throw new Exception("ID should not be null");
    }

    private Equipment AnEquipment() => new Equipment("Some equipment", EquipmentSlotType.WEAPON);
}