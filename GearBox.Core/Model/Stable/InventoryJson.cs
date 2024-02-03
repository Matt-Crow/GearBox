namespace GearBox.Core.Model.Stable;

public readonly struct InventoryJson : IJson
{
    public InventoryJson(InventoryTabJson equipment, InventoryTabJson materials)
    {
        Equipment = equipment;
        Materials = materials;
    }

    public InventoryTabJson Equipment { get; init; }
    public InventoryTabJson Materials { get; init; }
}