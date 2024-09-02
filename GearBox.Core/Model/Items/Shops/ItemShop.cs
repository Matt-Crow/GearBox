using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items.Shops;

public class ItemShop
{
    private readonly Inventory _stock;
    private readonly Dictionary<Guid, Inventory> _buyback = [];

    public ItemShop(Inventory? stock = null)
    {
        _stock = stock ?? new Inventory();
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
}