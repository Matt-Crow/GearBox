namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryJson : IChange, IJson
{
    public InventoryJson(InventoryTabJson manipulators, InventoryTabJson torsos, InventoryTabJson materials, int gold)
    {
        Manipulators = manipulators;
        Torsos = torsos;
        Materials = materials;
        Gold = gold;
    }

    public InventoryTabJson Manipulators { get; init; }
    public InventoryTabJson Torsos { get; init; }
    public InventoryTabJson Materials { get; init; }
    public int Gold { get; init; }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(Manipulators.DynamicValues)
        .Concat(Torsos.DynamicValues)
        .Concat(Materials.DynamicValues)
        .Append(Gold);
}