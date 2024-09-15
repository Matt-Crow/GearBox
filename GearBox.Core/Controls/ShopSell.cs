using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;

namespace GearBox.Core.Controls;

public class ShopSell : IControlCommand
{
    private readonly Guid _shopId;
    private readonly ItemSpecifier _specifier;

    public ShopSell(Guid shopId, ItemSpecifier specifier)
    {
        _shopId = shopId;
        _specifier = specifier;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        var area = target.CurrentArea ?? throw new Exception("Can only buy when in an area");
        var shop = area.Shops.Find(s => s.Id == _shopId) ?? throw new Exception($"Bad shop ID: {_shopId}");
        var item = target.Inventory.GetBySpecifier(_specifier);
        shop.BuyFrom(target, item);
    }
}
