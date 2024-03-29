using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A Material is a type of item which are only used for crafting
/// </summary>
public class Material : IItem
{
    public Material(ItemType type, string? description = null)
    {
        Type = type;
        Description = description ?? "no description provided";
    }

    public ItemType Type { get; init; }
    public string Description { get; init; }
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>(); // resources never change
    public List<KeyValueJson<string, object?>> Metadata { get; init; } = new(); // resources have no metadata
    public List<string> Tags { get; init; } = new(); // materials have no tags

    public InventoryTab GetTab(Inventory inventory)
    {
        return inventory.Materials;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Material;
        return other != null
            && other.Type.Equals(Type);
    }
    
    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}