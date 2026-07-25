namespace GearBox.Core.Utils.Factories;

/// <summary>
/// Useful adapter class.
/// e.g. KeyValue<string>() if you just want a Factory of strings
/// </summary>
public class FactoryProductWrapper<T> : IFactoryProduct
{
    public FactoryProductWrapper(string key, T value)
    {
        Key = key;
        Value = value;
    }

    public FactoryProductWrapper(T value)
    {
        Key = value?.ToString() ?? "null";
        Value = value;
    }


    public string Key { get; init; }
    public T Value { get; set; }
}