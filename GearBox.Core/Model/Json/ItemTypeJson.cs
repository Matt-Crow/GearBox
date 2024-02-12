namespace GearBox.Core.Model.Json;

public readonly struct ItemTypeJson : IJson
{
    public ItemTypeJson(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}