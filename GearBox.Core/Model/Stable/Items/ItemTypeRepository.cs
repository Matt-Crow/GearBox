using System.Collections.ObjectModel;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

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

    public List<ItemTypeJson> ToJson()
    {
        var result = _inner.Values
            .Select(x => x.ToJson())
            .ToList();
        return result;
    }
}