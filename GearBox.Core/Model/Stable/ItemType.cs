using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Every item has a specific type, which is used to group it with similar items.
/// </summary>
public readonly struct ItemType : ISerializable<ItemTypeJson>
{
    private ItemType(string name, bool isStackable)
    {
        Name = name;
        IsStackable = isStackable;
    }

    public static ItemType Stackable(string name) => new ItemType(name, true);
    public static ItemType NonStackable(string name) => new ItemType(name, false);

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
    /// This reduces the payload size when serializing Item to JSON.
    /// </summary>
    public ItemTypeJson ToJson()
    {
        return new ItemTypeJson(Name, IsStackable);
    }
}