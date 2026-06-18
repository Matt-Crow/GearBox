namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryJson : IChange, IJson
{
    public InventoryJson(List<ItemJson> equipment, InventoryTabJson materials, int gold)
    {
        Equipment = equipment;
        Materials = materials;
        Gold = gold;
    }

    public List<ItemJson> Equipment { get; init; }
    public InventoryTabJson Materials { get; init; }
    public int Gold { get; init; }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(Equipment.SelectMany(e => e.DynamicValues))
        .Concat(Materials.DynamicValues)
        .Append(Gold);
}