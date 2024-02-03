using System.Text.Json;

namespace GearBox.Core.Model.Stable;

public readonly struct InventoryItemJson : IJson
{
    public InventoryItemJson(string name, List<ItemMetadataJson> metadata, List<string> tags, int quantity)
    {
        Name = name;
        Metadata = metadata;
        Tags = tags;
        Quantity = quantity;
    }

    /// <summary>
    /// The front end uses this to lookup the item type in a repository
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Subclasses of InventoryItem should use this property to pass additional information to the front end
    /// </summary>
    public List<ItemMetadataJson> Metadata { get; init; }

    /// <summary>
    /// Subclasses of InventoryItem should use this property to pass additional information to the front end
    /// </summary>
    public List<string> Tags { get; init; }

    public int Quantity { get; init; }
}