namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct InventoryTabJson : IJson
{
    public InventoryTabJson(List<ItemJson> items)
    {
        Items = items;
    }

    public List<ItemJson> Items { get; init; }
}