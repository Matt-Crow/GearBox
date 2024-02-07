namespace GearBox.Core.Model.Json;

public readonly struct ItemTypeJson : IJson
{
    public ItemTypeJson(string name, bool isStackable)
    {
        Name = name;
        IsStackable = isStackable;
    }

    public string Name { get; init; }
    public bool IsStackable { get; init; }
}