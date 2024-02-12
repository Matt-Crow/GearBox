using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public class Equipment : IItem
{
    private readonly Guid _id;

    public Equipment(ItemType type, Guid? id = null)
    {
        Type = type;
        _id = id ?? Guid.NewGuid();
    }
    
    public ItemType Type { get; init; }

    /// <summary>
    /// Subclasses should override this if they can change at runtime
    /// </summary>
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>();

    /// <summary>
    /// Subclasses should override this
    /// </summary>
    public List<KeyValueJson<string, object?>> Metadata { get; init; } = new();

    /// <summary>
    /// Subclasses should override this
    /// </summary>
    public List<string> Tags { get; init; } = new();

    public InventoryTab GetTab(Inventory inventory)
    {
        return inventory.Equipment;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Equipment;
        return other?._id == _id;
    }
    
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }
}