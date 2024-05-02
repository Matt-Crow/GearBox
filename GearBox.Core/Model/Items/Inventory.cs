using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using System.Text.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IDynamic
{
    private readonly ChangeTracker _changeTracker;
    private bool _updatedLastFrame = true;

    public Inventory(Guid? ownerId = null)
    {
        OwnerId = ownerId ?? Guid.Empty;
        Serializer = new("inventory", Serialize);
        _changeTracker = new(this);
    }

    public Guid OwnerId { get; init; }
    public Serializer Serializer { get; init; }
    public InventoryTab<Weapon> Weapons { get; init; } = new();
    public InventoryTab<Material> Materials { get; init; } = new();
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>() 
        .Concat(Weapons.DynamicValues)
        .Concat(Materials.DynamicValues);

    /// <summary>
    /// Adds all items from the other inventory to this one
    /// </summary>
    public void Add(Inventory other)
    {
        // todo no stacks for weapons
        foreach (var weaponStack in other.Weapons.Content)
        {
            Weapons.Add(weaponStack.Item.ToOwned(), weaponStack.Quantity);
        }
        foreach (var materialStack in other.Materials.Content)
        {
            Materials.Add(materialStack.Item.ToOwned(), materialStack.Quantity);
        }
    }

    public bool Any()
    {
        return Weapons.Any() || Materials.Any();
    }

    public void Update()
    {
        _updatedLastFrame = _changeTracker.HasChanged;
        _changeTracker.Update();
    }

    private string Serialize(SerializationOptions options)
    {
        var json = new InventoryJson(
            OwnerId,
            Weapons.ToJson(),
            Materials.ToJson()
        );
        return JsonSerializer.Serialize(json, options.JsonSerializerOptions);
    }

    public StableJson ToJson(bool isWorldInit)
    {
        var result = _updatedLastFrame || isWorldInit
            ? StableJson.Changed(Serializer.Serialize(isWorldInit).Content)
            : StableJson.NoChanges();
        return result;
    }
}