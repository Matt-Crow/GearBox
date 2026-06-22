namespace GearBox.Core.Model.Items;

public class PartTab : InventoryTab<Part>
{
    public PartTab(PartSlotType slotType)
    {
        SlotType = slotType;
    }
    
    public PartSlotType SlotType { get; init; }
}