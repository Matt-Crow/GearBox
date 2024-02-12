using System.Text.Json;

namespace GearBox.Core.Model.Json;

/// <summary>
/// Combines together data from ItemStack, Item, and ItemType
/// </summary>
public readonly struct ItemJson : IJson
{
    // todo description
    
    public ItemJson(string name, List<KeyValueJson<string, object?>> metadata, List<string> tags, int quantity)
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
    /// Subclasses of Item should use this property to pass additional information to the front end
    /// </summary>
    public List<KeyValueJson<string, object?>> Metadata { get; init; }

    /// <summary>
    /// Subclasses of Item should use this property to pass additional information to the front end
    /// </summary>
    public List<string> Tags { get; init; }

    /// <summary>
    /// The number of items in this stack
    /// </summary>
    public int Quantity { get; init; }
}