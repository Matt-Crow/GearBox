using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct ChangesJson
{
    public ChangesJson(
        MaybeChangeJson<MapJson> map,
        MaybeChangeJson<List<ShopInitJson>> shops,
        MaybeChangeJson<InventoryJson> inventory,
        MaybeChangeJson<ItemJson?> weapon,
        MaybeChangeJson<ItemJson?> armor,
        MaybeChangeJson<PlayerStatSummaryJson> summary)
    {
        Map = map;
        Shops = shops;
        Inventory = inventory;
        Weapon = weapon;
        Armor = armor;
        Summary = summary;
    }

    /// <summary>
    /// Set whenever the player moves to a new area
    /// </summary>
    public MaybeChangeJson<MapJson> Map { get; init; }

    /// <summary>
    /// Set whenever the player moves to a new area
    /// </summary>
    public MaybeChangeJson<List<ShopInitJson>> Shops { get; init; }

    public MaybeChangeJson<InventoryJson> Inventory { get; init; }
    public MaybeChangeJson<ItemJson?> Weapon { get; init; }
    public MaybeChangeJson<ItemJson?> Armor { get; init; }
    public MaybeChangeJson<PlayerStatSummaryJson> Summary { get; init; }
}