using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public abstract class Equipment : IItem
{
    public Equipment(ItemType type, string? description = null, Guid? id = null)
    {
        Type = type;
        Description = description ?? "no description provided";
        Id = id ?? Guid.NewGuid();
    }
    
    public Guid Id { get; init; }
    
    public ItemType Type { get; init; }

    public string Description { get; init; }

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

    /// <summary>
    /// Returns the equipment slot this should be equipped in
    /// </summary>
    public abstract EquipmentSlot GetSlot(PlayerCharacter player);

    public override bool Equals(object? obj)
    {
        var other = obj as Equipment;
        return other?.Id == Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}