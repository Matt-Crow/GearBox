using System.Collections.ObjectModel;

namespace GearBox.Core.Model.Stable;

public class ItemTypeRepository : IItemTypeRepository
{
    private readonly ReadOnlyDictionary<string, ItemType> _inner;

    private ItemTypeRepository(Dictionary<string, ItemType> inner)
    {
        _inner = new ReadOnlyDictionary<string, ItemType>(inner);
    }

    public static ItemTypeRepository Of(IEnumerable<ItemType> itemTypes)
    {
        var asDictionary = itemTypes
            .ToDictionary<ItemType, string, ItemType>(x => x.Name, x => x);
        return new ItemTypeRepository(asDictionary);
    }

    public static ItemTypeRepository Empty() => new ItemTypeRepository(new Dictionary<string, ItemType>());

    public ItemType? GetByName(string name)
    {
        if (_inner.ContainsKey(name))
        {
            return _inner[name];
        }
        return null;
    }

    public IEnumerable<ItemType> GetAll()
    {
        return _inner.Values.AsEnumerable();
    }
}