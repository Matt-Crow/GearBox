namespace GearBox.Core.Utils.Factories;

/// <summary>
/// General purpose Factory.
/// </summary>
public class Factory<T>
where T : IFactoryProduct
{
    private readonly Dictionary<string, T> _values;
    private readonly Func<T, T> _toOwned;


    private Factory(Dictionary<string, T> values, Func<T, T> toOwned)
    {
        _values = values;
        _toOwned = toOwned;
    }


    public IEnumerable<T> AllValues => _values.Values;


    /// <summary>
    /// Creates a factory with the given ownership method and values.
    /// Duplicate keys throw an exception.
    /// </summary>
    /// <param name="toOwned">A method which converts the shared value to one the caller owns. For mutable data, this should return a copy.</param>
    /// <param name="values">The values this factory should be able to make.</param>
    /// <returns>the new factory</returns>
    public static Factory<T> Of(Func<T, T> toOwned, IEnumerable<T> values)
    {
        var dict = values.ToDictionary(v => v.Key);
        return new Factory<T>(dict, toOwned);
    }

    /// <summary>
    /// Returns an owned version of the value with the given key.
    /// Throws an exception if no such value exists.
    /// </summary>
    public T Make(string key)
    {
        _values.TryGetValue(key, out var value);
        if (value == null)
        {
            throw new ArgumentException($"Not found: '{key}'");
        }
        var owned = _toOwned(value);
        return owned;
    }
}