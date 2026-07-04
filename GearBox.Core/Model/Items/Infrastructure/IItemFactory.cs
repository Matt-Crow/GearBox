namespace GearBox.Core.Model.Items.Infrastructure;

/// <summary>
/// Makes items
/// </summary>
public interface IItemFactory
{
    /// <summary>
    /// Allows this to make the given item.
    /// </summary>
    IItemFactory Add(ItemUnion value);

    /// <summary>
    /// Returns a copy of the item with the given type name,
    /// or null if nothing is set for that key.
    /// </summary>
    ItemUnion? Make(string key);

    public ItemUnion MakeOrThrow(string key)
    {
        var item = Make(key) ?? throw new ArgumentException($"Invalid item name: {key}");
        return item;
    }
    
    public Material MakeMaterial(string key)
    {
        var item = Make(key) ?? throw new ArgumentException($"No item registered with name '{key}'");
        
        Material? material = null;
        item.Match(
            m => material = m,
            _ => {}
        );
        if (material == null)
        {
            throw new ArgumentException($"Item with name '{key}' is not a material");
        }

        return material;
    }
}