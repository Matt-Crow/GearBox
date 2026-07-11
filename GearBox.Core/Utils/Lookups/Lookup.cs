namespace GearBox.Core.Utils.Lookups;

/// <summary>
/// General purpose lookup table.
/// </summary>
public class Lookup<T>
where T : ILookupValue
{
    private readonly Dictionary<string, T> _values;
    private readonly Func<T, T> _toOwned;
    private readonly Func<T, T> SELF = (x) => x;


    private Lookup(Dictionary<string, T> values, Func<T, T>? toOwned)
    {
        _values = values;
        _toOwned = toOwned ?? SELF;
    }


    /// <summary>
    /// Creates a lookup with the given value.
    /// Duplicate keys throw an exception.
    /// You can additionally provide a function to copy values retrieved from this.
    /// </summary>
    public static Lookup<T> Of(IEnumerable<T> values, Func<T, T>? toOwned = null)
    {
        var dict = values.ToDictionary(v => v.Key);
        return new Lookup<T>(dict, toOwned);
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
}