namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct ChangesJson
{
    public ChangesJson(
        MaybeChangeJson<InventoryJson> inventory,
        MaybeChangeJson<ItemJson?> weapon,
        MaybeChangeJson<ItemJson?> armor,
        MaybeChangeJson<PlayerStatSummaryJson> summary)
    {
        Inventory = inventory;
        Weapon = weapon;
        Armor = armor;
        Summary = summary;
    }

    public MaybeChangeJson<InventoryJson> Inventory { get; init; }
    public MaybeChangeJson<ItemJson?> Weapon { get; init; }
    public MaybeChangeJson<ItemJson?> Armor { get; init; }
    public MaybeChangeJson<PlayerStatSummaryJson> Summary { get; init; }
}