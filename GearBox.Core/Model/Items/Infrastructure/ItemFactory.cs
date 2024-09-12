namespace GearBox.Core.Model.Items.Infrastructure;

public class ItemFactory : IItemFactory
{
    private readonly Dictionary<string, ItemUnion> _items = [];

    public IItemFactory Add(ItemUnion value)
    {
        _items[value.Name] = value;
        return this;
    }

    public ItemUnion? Make(string key)
    {
        if (_items.TryGetValue(key, out ItemUnion? value))
        {
            return value.ToOwned();
        }
        return null;
    }
}
