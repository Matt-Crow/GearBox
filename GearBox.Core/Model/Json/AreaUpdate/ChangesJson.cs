using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct ChangesJson
{
    public ChangesJson(
        MaybeChangeJson<MapJson> map,
        MaybeChangeJson<InventoryJson> inventory,
        MaybeChangeJson<ItemJson?> weapon,
        MaybeChangeJson<ItemJson?> armor,
        MaybeChangeJson<PlayerStatSummaryJson> summary)
    {
        Map = map;
        Inventory = inventory;
        Weapon = weapon;
        Armor = armor;
        Summary = summary;
    }

    /// <summary>
    /// Set whenever the player moves to a new area
    /// </summary>
    public MaybeChangeJson<MapJson> Map { get; init; }

    public MaybeChangeJson<InventoryJson> Inventory { get; init; }
    public MaybeChangeJson<ItemJson?> Weapon { get; init; }
    public MaybeChangeJson<ItemJson?> Armor { get; init; }
    public MaybeChangeJson<PlayerStatSummaryJson> Summary { get; init; }
}