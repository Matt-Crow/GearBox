namespace GearBox.Core.Model.Json.AreaUpdate;

public class UiStateChangesJson
{
    public MaybeChangeJson<AreaJson?> Area { get; set; }
    public MaybeChangeJson<InventoryJson> Inventory { get; set; }
    public MaybeChangeJson<ItemJson?> Weapon { get; set; }
    public MaybeChangeJson<ItemJson?> Armor { get; set; }
    public MaybeChangeJson<PlayerStatSummaryJson> Summary { get; set; }
    public MaybeChangeJson<List<ActiveAbilityJson>> Actives { get; set; }
    public MaybeChangeJson<OpenShopJson?> OpenShop { get; set; }
}