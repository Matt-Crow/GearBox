namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public interface IMightChange<TJson>
{
    /// <summary>
    /// Values on this game object which can change over time.
    /// </summary>
    IEnumerable<object?> DynamicValues { get; }

    /// <summary>
    /// Called when the object changes
    /// </summary>
    TJson ToJson();
}