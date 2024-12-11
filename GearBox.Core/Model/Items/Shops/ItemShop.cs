using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json.AreaUpdate;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items.Shops;

public class ItemShop
{
    private readonly string _name;
    private readonly Coordinates _coordinates;
    private readonly Color _color;
    private readonly Inventory _stock;
    private readonly Dictionary<Guid, Inventory> _buyback = [];
    private readonly BodyBehavior _body; // todo collider

    public ItemShop(string name, Coordinates coordinates, Color color, Inventory stock)
    {
        _name = name;
        _coordinates = coordinates;
        _color = color;
        _stock = stock;
        _body = new BodyBehavior()
        {
            Location = _coordinates.CenteredOnTile()
        };
        Id = Guid.NewGuid();
    }

    public ItemShop(Inventory? stock = null) : this("a shop", Coordinates.ORIGIN, Color.BLUE, stock ?? new Inventory())
    {
    }

    public Guid Id { get; init; }

    public void SellTo(PlayerCharacter player, ItemSpecifier specifier)
    {
        var item = GetBuybackOptionsFor(player).GetBySpecifier(specifier) ?? _stock.GetBySpecifier(specifier);
        SellTo(player, item);
    }

    /// <summary>
    /// Sells the given item to the given player
    /// </summary>
    public void SellTo(PlayerCharacter player, ItemUnion? item)
    {
        if (item == null)
        {
            return;
        }
        if (player.Inventory.Gold.Quantity < item.BuyValue.Quantity)
        {
            return; // insufficient funds
        }

        if (GetBuybackOptionsFor(player).Contains(item))
        {
            player.Inventory.Add(item.ToOwned());
            player.Inventory.Remove(item.BuyValue);
            GetBuybackOptionsFor(player).Remove(item);
        }

        if (_stock.Contains(item))
        {
            player.Inventory.Add(item.ToOwned());
            player.Inventory.Remove(item.BuyValue);
        }
    }

    /// <summary>
    /// Buys the given item from the given player
    /// </summary>
    public void BuyFrom(PlayerCharacter player, ItemUnion? item)
    {
        if (item == null)
        {
            return;
        }

        if (!player.Inventory.Contains(item))
        {
            return;
        }

        player.Inventory.Remove(item);
        player.Inventory.Add(item.BuyValue);

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

    public bool CollidesWith(PlayerCharacter player) => _body.CollidesWith(player.Body);

    public ShopInitJson ToJson()
    {
        var result = new ShopInitJson(
            _name,
            _coordinates.XInPixels,
            _coordinates.YInPixels,
            _color.ToJson()
        );
        return result;
    }

    public OpenShopJson GetOpenShopJsonFor(PlayerCharacter player)
    {
        var result = new OpenShopJson(
            Id,
            _name,
            player.Inventory.Gold.Quantity,
            GetOptionsFrom(_stock, player.Inventory.Gold), 
            GetOptionsFrom(player.Inventory, null), // don't check whether player can afford to sell
            GetOptionsFrom(GetBuybackOptionsFor(player), player.Inventory.Gold)
        );
        return result;
    }

    private static List<OpenShopOptionJson> GetOptionsFrom(Inventory inventory, Gold? playerGold)
    {
        var result = new List<OpenShopOptionJson>()
            .Concat(GetOptionsFrom(inventory.Materials, playerGold))
            .Concat(GetOptionsFrom(inventory.Weapons, playerGold))
            .Concat(GetOptionsFrom(inventory.Armors, playerGold))
            .ToList();
        return result;
    }

    private static List<OpenShopOptionJson> GetOptionsFrom<T>(InventoryTab<T> tab, Gold? playerGold)
    where T : IItem
    {
        // null for gold means "don't bother checking if the player can afford it"
        var result = new List<OpenShopOptionJson>();
        foreach (var item in tab.Content)
        {
            var opt = new OpenShopOptionJson(
                item.ToJson(), 
                item.Item.BuyValue.Quantity,
                playerGold == null || playerGold.Quantity >= item.Item.BuyValue.Quantity
            );
            result.Add(opt);
        }
        return result;
    }
}