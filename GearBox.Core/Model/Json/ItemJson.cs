namespace GearBox.Core.Model.Json;

/// <summary>
/// Combines together data from ItemStack, IItem, and ItemType
/// </summary>
public readonly struct ItemJson : IJson
{
    public ItemJson(Guid? id, string name, string description, IEnumerable<string> details, int quantity)
    {
        Id = id;
        Name = name;
        Description = description;
        Details = details;
        Quantity = quantity;
    }

    public Guid? Id { get; init; }

    /// <summary>
    /// The front end uses this to lookup the item type in a repository
    /// </summary>
    public string Name { get; init; }

    public string Description { get; init; }
    public IEnumerable<string> Details { get; init; }

    /// <summary>
    /// The number of items in this stack
    /// </summary>
    public int Quantity { get; init; }
}