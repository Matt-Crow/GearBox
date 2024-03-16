namespace GearBox.Core.Model.Stable;

public interface IDynamic
{
    /// <summary>
    /// Values on this game object which can change over time.
    /// </summary>
    IEnumerable<object?> DynamicValues { get; }
}