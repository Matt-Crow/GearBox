using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Something which can exist in a player's inventory.
/// </summary>
public interface IInventoryItemType
{
    /// <summary>
    /// Used by the frontend to deserialize
    /// </summary>
    public string ItemType { get; }

    /// <summary>
    /// Whether multiple of this item type can exist in a stack
    /// </summary>
    public bool IsStackable { get; }

    /// <summary>
    /// Serializes this item type so it can be sent to the frontend
    /// </summary>
    public string Serialize(JsonSerializerOptions options);
}