namespace GearBox.Core.Model.Items.Infrastructure;

/// <summary>
/// Makes items
/// </summary>
public interface IItemFactory
{
    /// <summary>
    /// All different types of items this can make
    /// </summary>
    IEnumerable<ItemType> AvailableTypes { get; }

    /// <summary>
    /// Allows this to make the given item.
    /// </summary>
    IItemFactory Add(ItemUnion value);

    /// <summary>
    /// Returns a copy of the item with the given type name,
    /// or null if nothing is set for that key.
    /// </summary>
    ItemUnion? Make(string key);
}