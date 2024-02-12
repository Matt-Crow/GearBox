using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// an item which can go in a player's inventory
/// </summary>
public class Item 
{
    public Item(ItemType type)
    {
        ItemType = type;
    }
    
    public ItemType ItemType {get; init; }

    // todo description

    /// <summary>
    /// Subclasses should override this method if they need to provide metadata to the front end
    /// </summary>
    public virtual List<KeyValueJson<string, object?>> Metadata => new();
    
    /// <summary>
    /// Subclasses should override this method if they need to provide tags to the front end
    /// </summary>
    public virtual List<string> Tags => new();

    // todo figure out how to handle items with dynamic metadata & tags
    // better to make this virtual, subclasses handle it
    public IEnumerable<object?> DynamicValues => Metadata.AsEnumerable()
        .OrderBy(kv => kv.Key)
        .Select(kv => $"{kv.Key}: {kv.Value}")
        .Concat(Tags);

    // todo method of getting which inventory tab to add to / remove from

    public bool Is(Item other)
    {
        var sameItemType = other.ItemType.Equals(ItemType);
        var sameMetadata = other.Metadata.Count == Metadata.Count
            && !other.Metadata.Except(Metadata).Any();
        var sameTags = other.Tags.Count == Tags.Count
            && !other.Tags.Except(Tags).Any();
        return sameItemType && sameMetadata && sameTags;
    }
}