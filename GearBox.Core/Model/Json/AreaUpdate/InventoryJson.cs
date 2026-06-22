namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryJson : IChange, IJson
{
    public InventoryJson(List<ItemJson> parts, InventoryTabJson materials, int gold)
    {
        Parts = parts;
        Materials = materials;
        Gold = gold;
    }

    public List<ItemJson> Parts { get; init; }
    public InventoryTabJson Materials { get; init; }
    public int Gold { get; init; }

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(Parts.SelectMany(e => e.DynamicValues))
        .Concat(Materials.DynamicValues)
        .Append(Gold);
}