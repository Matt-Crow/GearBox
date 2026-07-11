namespace GearBox.Core.Utils.Factories;

/// <summary>
/// General purpose Factory.
/// </summary>
public class Factory<T>
where T : IFactoryProduct
{
    private readonly Dictionary<string, T> _values;
    private readonly Func<T, T> _toOwned;
    private readonly Func<T, T> SELF = (x) => x;


    private Factory(Dictionary<string, T> values, Func<T, T>? toOwned)
    {
        _values = values;
        _toOwned = toOwned ?? SELF;
    }


    /// <summary>
    /// Creates a factory with the given values.
    /// Duplicate keys throw an exception.
    /// You can additionally provide a function to copy values retrieved from this.
    /// </summary>
    public static Factory<T> Of(IEnumerable<T> values, Func<T, T>? toOwned = null)
    {
        var dict = values.ToDictionary(v => v.Key);
        return new Factory<T>(dict, toOwned);
    }

    /// <summary>
    /// Returns the value with the given key,
    /// or null if no such value exists.
    /// </summary>
    public T? Get(string key)
    {
        _values.TryGetValue(key, out var result);
        if (result != null)
        {
            return _toOwned(result);
        }
        return result;
    }

    public T GetOrThrow(string key)
    {
        return Get(key) ?? throw new ArgumentException($"Not found: '{key}'");
    }
}