namespace GearBox.Core.Model.Stable;

public readonly struct InventoryItemTypeJson : IJson
{
    public InventoryItemTypeJson(string name, bool isStackable)
    {
        Name = name;
        IsStackable = isStackable;
    }

    public string Name { get; init; }
    public bool IsStackable { get; init; }
}