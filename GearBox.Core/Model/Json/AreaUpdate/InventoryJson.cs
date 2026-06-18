namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryJson : IChange, IJson
{
    public InventoryJson(InventoryTabJson weapons, InventoryTabJson torsos, InventoryTabJson materials, int gold)
    {
        Weapons = weapons;
        Torsos = torsos;
        Materials = materials;
        Gold = gold;
    }

    public InventoryTabJson Weapons { get; init; }
    public InventoryTabJson Torsos { get; init; }
    public InventoryTabJson Materials { get; init; }
    public int Gold { get; init; }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(Weapons.DynamicValues)
        .Concat(Torsos.DynamicValues)
        .Concat(Materials.DynamicValues)
        .Append(Gold);
}