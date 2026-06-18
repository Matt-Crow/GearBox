namespace GearBox.Core.Model.Items;

public class EquipmentTab : InventoryTab<Equipment>
{
    public EquipmentTab(EquipmentSlotType slotType)
    {
        SlotType = slotType;
    }
    
    public EquipmentSlotType SlotType { get; init; }
}