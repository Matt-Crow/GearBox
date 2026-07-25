using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class ItemShopResource
{
    public required string Name { get; set; }
    public required int XInTiles { get; set; }
    public required int YInTiles { get; set; }
    public required string Color { get; set; }
    public required List<string> Stock { get; set; }

    
    public ItemShop ToItemShop(Factory<ItemUnion> items)
    {
        var stock = new Inventory();
        foreach (var itemName in Stock)
        {
            stock.Add(items.Make(itemName));
        }

        var shop = new ItemShop(
            Name,
            Coordinates.FromTiles(XInTiles, YInTiles),
            GearBox.Core.Model.Color.FromName(Color) ?? throw new Exception($"Invalid color name: '{Color}'"),
            stock
        );

        return shop;
    }
}