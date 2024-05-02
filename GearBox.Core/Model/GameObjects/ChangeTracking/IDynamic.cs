namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public interface IDynamic
{
    /// <summary>
    /// Values on this game object which can change over time.
    /// </summary>
    IEnumerable<object?> DynamicValues { get; }
}