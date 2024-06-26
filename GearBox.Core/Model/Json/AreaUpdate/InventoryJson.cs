namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct InventoryJson : IJson
{
    public InventoryJson(InventoryTabJson weapons, InventoryTabJson armors, InventoryTabJson materials)
    {
        Weapons = weapons;
        Armors = armors;
        Materials = materials;
    }

    public InventoryTabJson Weapons { get; init; }
    public InventoryTabJson Armors { get; init; }
    public InventoryTabJson Materials { get; init; }
}