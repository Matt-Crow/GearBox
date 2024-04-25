using GearBox.Core.Model.Json;
using System.Text.Json;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IStableGameObject
{
    public Inventory(Guid? ownerId = null)
    {
        OwnerId = ownerId ?? Guid.Empty;
        Serializer = new("inventory", Serialize);
    }

    public Guid OwnerId { get; init; }
    public Serializer Serializer { get; init; }
    public InventoryTab<Equipment> Equipment { get; init; } = new();
    public InventoryTab<Material> Materials { get; init; } = new();
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>() 
        .Concat(Equipment.DynamicValues)
        .Concat(Materials.DynamicValues);

    /// <summary>
    /// Adds all items from the other inventory to this one
    /// </summary>
    public void Add(Inventory other)
    {
        // todo no stacks for equipment
        foreach (var equipmentStack in other.Equipment.Content)
        {
            // do I need to clone equipment to avoid duplication?
            Equipment.Add(equipmentStack.Item, equipmentStack.Quantity);
        }
        foreach (var materialStack in other.Materials.Content)
        {
            Materials.Add(materialStack.Item, materialStack.Quantity);
        }
    }

    public bool Any()
    {
        return Equipment.Any() || Materials.Any();
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