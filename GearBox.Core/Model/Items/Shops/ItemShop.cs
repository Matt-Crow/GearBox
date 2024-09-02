using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json.AreaInit;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items.Shops;

public class ItemShop
{
    private readonly string _name;
    private readonly Coordinates _coordinates;
    private readonly Color _color;
    private readonly Inventory _stock;
    private readonly Dictionary<Guid, Inventory> _buyback = [];

    public ItemShop(string name, Coordinates coordinates, Color color, Inventory stock)
    {
        _name = name;
        _coordinates = coordinates;
        _color = color;
        _stock = stock;
    }

    public ItemShop(Inventory? stock = null) : this("a shop", Coordinates.ORIGIN, Color.BLUE, stock ?? new Inventory())
    {
    }

    /// <summary>
    /// Sells the given item to the given player
    /// </summary>
    public void SellTo(PlayerCharacter player, ItemUnion item)
    {
        if (player.Inventory.Gold.Quantity < item.BuyValue().Quantity)
        {
            return; // insufficient funds
        }

        if (GetBuybackOptionsFor(player).Contains(item))
        {
            player.Inventory.Add(item.ToOwned());
            player.Inventory.Remove(item.BuyValue());
            GetBuybackOptionsFor(player).Remove(item);
        }

        if (_stock.Contains(item))
        {
            player.Inventory.Add(item.ToOwned());
            player.Inventory.Remove(item.BuyValue());
        }
    }

    /// <summary>
    /// Buys the given item from the given player
    /// </summary>
    public void BuyFrom(PlayerCharacter player, ItemUnion item)
    {
        if (!player.Inventory.Contains(item))
        {
            return;
        }

        player.Inventory.Remove(item);
        player.Inventory.Add(item.BuyValue());

        if (!_buyback.ContainsKey(player.Id))
        {
            _buyback[player.Id] = new Inventory();
        }

        _buyback[player.Id].Add(item);
    }

    public Inventory GetBuybackOptionsFor(PlayerCharacter player)
    {
        if (_buyback.TryGetValue(player.Id, out Inventory? value))
        {
            return value;
        }
        return new Inventory();
    }

    public ShopInitJson ToJson()
    {
        var result = new ShopInitJson(
            _name,
            _coordinates.XInPixels,
            _coordinates.YInPixels,
            _color.ToJson(),
            _stock.ToJson()
        );
        return result;
    }
}