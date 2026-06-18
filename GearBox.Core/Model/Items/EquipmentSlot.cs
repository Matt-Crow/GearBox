namespace GearBox.Core.Model.Items;

/// <summary>
/// A place where a player can slot in a specific type of equipment.
/// Essentially acts as a pointer to an Equipment when a layer of indirection is needed.
/// </summary>
public class EquipmentSlot
{
    public EquipmentSlot(EquipmentSlotType slotType, Equipment? equipment = null)
    {
        SlotType = slotType;
        Equipment = equipment;
    }

    /// <summary>
    /// The type of equipment which can go in this slot.
    /// </summary>
    public EquipmentSlotType SlotType { get; init; }

    /// <summary>
    /// The equipment currently in this slot.
    /// </summary>
    public Equipment? Equipment { get; set; }
}