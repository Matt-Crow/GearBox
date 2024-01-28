namespace GearBox.Core.Model.Stable;

/// <summary>
/// Every inventory item has a specific type, which is used to group it with similar items.
/// </summary>
public readonly struct InventoryItemType
{
    private InventoryItemType(string name, bool isStackable)
    {
        Name = name;
        IsStackable = isStackable;
    }

    public static InventoryItemType Stackable(string name) => new InventoryItemType(name, true);
    public static InventoryItemType NonStackable(string name) => new InventoryItemType(name, false);

    /// <summary>
    /// Used by the frontend to deserialize
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Whether multiple items of this type can exist in a stack in a player's inventory
    /// </summary>
    public bool IsStackable { get; init; }
}