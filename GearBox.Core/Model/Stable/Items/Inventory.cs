using GearBox.Core.Model.Json;
using System.Text.Json;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IStableGameObject
{
    public Inventory(Guid ownerId)
    {
        OwnerId = ownerId;
        Serializer = new("inventory", Serialize);
    }

    public Guid OwnerId { get; init; }
    public Serializer Serializer { get; init; }
    public InventoryTab Equipment { get; init; } = new();
    public InventoryTab Materials { get; init; } = new();
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>() 
        .Concat(Equipment.DynamicValues)
        .Concat(Materials.DynamicValues);


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
        // do nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        var json = new InventoryJson(
            OwnerId,
            Equipment.ToJson(),
            Materials.ToJson()
        );
        return JsonSerializer.Serialize(json, options);
    }
}