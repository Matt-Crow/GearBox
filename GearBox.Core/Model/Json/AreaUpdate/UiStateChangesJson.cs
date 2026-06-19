namespace GearBox.Core.Model.Json.AreaUpdate;

public class UiStateChangesJson
{
    public MaybeChangeJson<AreaJson?> Area { get; set; }
    public MaybeChangeJson<InventoryJson> Inventory { get; set; }
    public List<EquipmentSlotJson> EquipmentSlots { get; set; } = [];
    public MaybeChangeJson<PlayerStatSummaryJson> Summary { get; set; }
    public MaybeChangeJson<List<ActiveAbilityJson>> Actives { get; set; }
    public MaybeChangeJson<List<PassiveAbilityJson>> Passives { get; set; }
    public MaybeChangeJson<OpenShopJson?> OpenShop { get; set; }
}