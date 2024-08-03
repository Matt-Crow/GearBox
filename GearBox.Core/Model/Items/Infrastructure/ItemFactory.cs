namespace GearBox.Core.Model.Items.Infrastructure;

public class ItemFactory : IItemFactory
{
    private readonly Dictionary<string, ItemUnion> _items = [];

    public IEnumerable<ItemType> AvailableTypes => _items.Values.Select(u => u.GetItemType());

    public IItemFactory Add(ItemUnion value)
    {
        _items[value.GetItemType().Name] = value;
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
