namespace GearBox.Core.Model.Stable;

public readonly struct ItemMetadataJson : IJson
{
    public ItemMetadataJson(string key, object? value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public object? Value { get; init; }
} 