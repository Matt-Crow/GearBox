namespace GearBox.Core.Model.Json.AreaUpdate;

public class EquipmentSlotJson
{
    public EquipmentSlotJson(string slotType, MaybeChangeJson<ItemJson?> equipment)
    {
        SlotType = slotType;
        Equipment = equipment;
    }

    public string SlotType { get; init; }
    public MaybeChangeJson<ItemJson?> Equipment { get; init; }
}