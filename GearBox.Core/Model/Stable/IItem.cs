using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

public interface IItem
{
    /// <summary>
    /// Items of the same type can stack together in a player's inventory
    /// </summary>
    ItemType Type { get; }

    /// <summary>
    /// Values which may change throughout the life of an IItem
    /// </summary>
    IEnumerable<object?> DynamicValues { get; }

    /// <summary>
    /// Provides key-value pairs to the frontend
    /// </summary>
    List<KeyValueJson<string, object?>> Metadata { get; }

    /// <summary>
    /// Provides value data to the frontend
    /// </summary>
    List<string> Tags { get; }

    /// <summary>
    /// Gets the inventory tab this belongs in
    /// </summary>
    InventoryTab GetTab(Inventory inventory);
}