namespace GearBox.Core.Model.Stable;

public readonly struct InventoryTabJson : IJson
{
    public InventoryTabJson(List<InventoryItemJson> items)
    {
        Items = items;
    }

    public List<InventoryItemJson> Items { get; init; }
}