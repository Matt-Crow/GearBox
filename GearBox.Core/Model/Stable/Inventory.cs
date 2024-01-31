using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IStableGameObject, ISerializable<InventoryJson>
{
    public InventoryTab Equipment { get; init; } = new();
    public InventoryTab Materials { get; init; } = new();
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>() 
        .Concat(Equipment.DynamicValues)
        .Concat(Materials.DynamicValues);
    public string Type => "inventory";

    public void Update()
    {
        // does nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public InventoryJson ToJson()
    {
        var result = new InventoryJson(
            Equipment.ToJson(),
            Materials.ToJson()
        );
        return result;
    }
}