namespace GearBox.Core.Model.Stable;

/// <summary>
/// Every inventory item has a specific type, which is used to group it with similar items.
/// </summary>
public readonly struct InventoryItemType : ISerializable<InventoryItemTypeJson>
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

    /// <summary>
    /// Item types should only be serialized when sending the world init message,
    /// as the client can then use a repository to lookup the item type definition by name.
    /// This reduces the payload size when serializing InventoryItem to JSON.
    /// </summary>
    public InventoryItemTypeJson ToJson()
    {
        return new InventoryItemTypeJson(Name, IsStackable);
    }
}