namespace GearBox.Core.Utils.Lookups;

/// <summary>
/// Useful adapter class.
/// e.g. KeyValue<string>() if you just want a lookup of strings
/// </summary>
public class KeyValue<T> : ILookupValue
{
    public KeyValue(string key, T value)
    {
        Key = key;
        Value = value;
    }

    public KeyValue(T value)
    {
        Key = value?.ToString() ?? "null";
        Value = value;
    }


    public string Key { get; init; }
    public T Value { get; set; }
}