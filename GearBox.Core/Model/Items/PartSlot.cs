namespace GearBox.Core.Model.Items;

/// <summary>
/// A place where a player can slot in a specific type of part.
/// Essentially acts as a pointer to an Part when a layer of indirection is needed.
/// </summary>
public class PartSlot
{
    public PartSlot(PartSlotType slotType, Part? part = null)
    {
        SlotType = slotType;
        Part = part;
    }

    /// <summary>
    /// The type of part which can go in this slot.
    /// </summary>
    public PartSlotType SlotType { get; init; }

    /// <summary>
    /// The part currently in this slot.
    /// </summary>
    public Part? Part { get; set; }
}