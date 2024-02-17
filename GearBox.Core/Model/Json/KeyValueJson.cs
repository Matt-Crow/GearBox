namespace GearBox.Core.Model.Json;

/// <summary>
/// represents a simple key-to-value mapping in JSON
/// </summary>
/// <typeparam name="TKey">the type of key</typeparam>
/// <typeparam name="TValue">the type of value</typeparam>
public readonly struct KeyValueJson<TKey, TValue>
{
    public KeyValueJson(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public TKey Key { get; init; }
    public TValue? Value { get; init; }
}