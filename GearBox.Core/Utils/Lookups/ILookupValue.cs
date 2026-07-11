namespace GearBox.Core.Utils.Lookups;

/// <summary>
/// A value which can go in a Lookup.
/// </summary>
public interface ILookupValue
{
    /// <summary>
    /// Used to look up this value.
    /// </summary>
    string Key { get; }
}