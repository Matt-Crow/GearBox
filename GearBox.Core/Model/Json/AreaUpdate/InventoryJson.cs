namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryJson : IChange, IJson
{
    public InventoryJson(InventoryTabJson weapons, InventoryTabJson armors, InventoryTabJson materials, int gold)
    {
        Weapons = weapons;
        Armors = armors;
        Materials = materials;
        Gold = gold;
    }

    public InventoryTabJson Weapons { get; init; }
    public InventoryTabJson Armors { get; init; }
    public InventoryTabJson Materials { get; init; }
    public int Gold { get; init; }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(Weapons.DynamicValues)
        .Concat(Armors.DynamicValues)
        .Concat(Materials.DynamicValues)
        .Append(Gold);
}