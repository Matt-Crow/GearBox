using GearBox.Core.Model.Json;
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

    public void Add(IItem item)
    {
        var tabToAddTo = item.GetTab(this);
        tabToAddTo.Add(item);
    }

    public void Remove(IItem item)
    {
        var tabToRemoveFrom = item.GetTab(this);
        tabToRemoveFrom.Remove(item);
    }

    public bool Contains(IItem item)
    {
        var tabToCheck = item.GetTab(this);
        return tabToCheck.Contains(item);
    }

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