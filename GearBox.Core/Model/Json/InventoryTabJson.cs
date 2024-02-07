namespace GearBox.Core.Model.Json;

public readonly struct InventoryTabJson : IJson
{
    public InventoryTabJson(List<ItemJson> items)
    {
        Items = items;
    }

    public List<ItemJson> Items { get; init; }
}