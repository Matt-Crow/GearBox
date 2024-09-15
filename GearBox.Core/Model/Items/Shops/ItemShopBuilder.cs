using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items.Shops;

public class ItemShopBuilder
{
    private readonly string _name;
    private readonly Coordinates _coordinates;
    private readonly Color _color;
    private readonly IItemFactory _itemFactory;
    private readonly List<string> _itemNames = [];

    public ItemShopBuilder(string name, Coordinates coordinates, Color color, IItemFactory itemFactory)
    {
        _name = name;
        _coordinates = coordinates;
        _color = color;
        _itemFactory = itemFactory;
    }

    public ItemShopBuilder AddItem(string name)
    {
        _itemNames.Add(name);
        return this;
    }

    public ItemShop Build()
    {
        var stock = new Inventory();
        foreach (var itemName in _itemNames)
        {
            var item = _itemFactory.Make(itemName) ?? throw new Exception($"Bad item name: {itemName}");
            stock.Add(item);
        }
        var result = new ItemShop(
            _name,
            _coordinates,
            _color,
            stock
        );
        return result;
    }
}