namespace GearBox.Core.Model.Json.AreaUpdate;

public class PartSlotJson
{
    public PartSlotJson(string slotType, MaybeChangeJson<ItemJson?> part)
    {
        SlotType = slotType;
        Part = part;
    }

    public string SlotType { get; init; }
    public MaybeChangeJson<ItemJson?> Part { get; init; }
}