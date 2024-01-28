using System.Collections.ObjectModel;

namespace GearBox.Core.Model.Stable;

public class InventoryItemTypeRepository : IInventoryItemTypeRepository
{
    private readonly ReadOnlyDictionary<string, InventoryItemType> _inner;

    private InventoryItemTypeRepository(Dictionary<string, InventoryItemType> inner)
    {
        _inner = new ReadOnlyDictionary<string, InventoryItemType>(inner);
    }

    public static InventoryItemTypeRepository Of(IEnumerable<InventoryItemType> itemTypes)
    {
        var asDictionary = itemTypes
            .ToDictionary<InventoryItemType, string, InventoryItemType>(x => x.Name, x => x);
        return new InventoryItemTypeRepository(asDictionary);
    }

    public InventoryItemType? GetByName(string name)
    {
        if (_inner.ContainsKey(name))
        {
            return _inner[name];
        }
        return null;
    }

    public IEnumerable<InventoryItemType> GetAll()
    {
        return _inner.Values.AsEnumerable();
    }
}