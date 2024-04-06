namespace GearBox.Core.Model.Json;

public readonly struct InventoryJson : IJson
{
    public InventoryJson(Guid ownerId, InventoryTabJson equipment, InventoryTabJson materials)
    {
        OwnerId = ownerId;
        Equipment = equipment;
        Materials = materials;
    }

    public Guid OwnerId { get; init; }
    public InventoryTabJson Equipment { get; init; }
    public InventoryTabJson Materials { get; init; }
}