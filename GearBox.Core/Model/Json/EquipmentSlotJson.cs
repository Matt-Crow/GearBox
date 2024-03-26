namespace GearBox.Core.Model.Json;

public readonly struct EquipmentSlotJson : IJson
{
    public EquipmentSlotJson(Guid ownerId, ItemJson? value)
    {
        OwnerId = ownerId;
        Value = value;
    }

    public Guid OwnerId { get; init; }
    public ItemJson? Value { get; init; }
}