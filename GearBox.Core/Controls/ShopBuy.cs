using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;

namespace GearBox.Core.Controls;

public class ShopBuy : IControlCommand
{
    private readonly Guid _shopId;
    private readonly ItemSpecifier _specifier;

    public ShopBuy(Guid shopId, ItemSpecifier specifier)
    {
        _shopId = shopId;
        _specifier = specifier;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        var area = target.CurrentArea ?? throw new Exception("Can only buy when in an area");
        var shop = area.Shops.Find(s => s.Id == _shopId) ?? throw new Exception($"Bad shop ID: {_shopId}");
        shop.SellTo(target, _specifier);
    }
}
