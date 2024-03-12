using System.Text.Json;

namespace GearBox.Core.Model.Json;

/// <summary>
/// Combines together data from ItemStack, IItem, and ItemType
/// </summary>
public readonly struct ItemJson : IJson
{
    public ItemJson(Guid? id, string name, string description, List<KeyValueJson<string, object?>> metadata, List<string> tags, int quantity)
    {
        Id = id;
        Name = name;
        Description = description;
        Metadata = metadata;
        Tags = tags;
        Quantity = quantity;
    }

    public Guid? Id { get; init; }

    /// <summary>
    /// The front end uses this to lookup the item type in a repository
    /// </summary>
    public string Name { get; init; }

    public string Description { get; init; }

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