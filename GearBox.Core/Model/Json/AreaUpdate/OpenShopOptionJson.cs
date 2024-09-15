namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct OpenShopOptionJson
{
    public OpenShopOptionJson(ItemJson item, int buyPrice, bool canAfford)
    {
        Item = item;
        BuyPrice = buyPrice;
        CanAfford = canAfford;
    }

    public ItemJson Item { get; init; }
    public int BuyPrice { get; init; }
    public bool CanAfford { get; init; }
}