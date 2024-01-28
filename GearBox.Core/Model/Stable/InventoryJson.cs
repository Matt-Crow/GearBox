namespace GearBox.Core.Model.Stable;

public readonly struct InventoryJson : IJson
{
    public InventoryJson(List<InventoryItemJson> items)
    {
        Items = items;
    }

    public List<InventoryItemJson> Items { get; init; }
}