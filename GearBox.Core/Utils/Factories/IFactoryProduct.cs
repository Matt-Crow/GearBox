namespace GearBox.Core.Utils.Factories;

/// <summary>
/// A value which can go in a Factory.
/// </summary>
public interface IFactoryProduct
{
    /// <summary>
    /// Used to make this value.
    /// </summary>
    string Key { get; }
}