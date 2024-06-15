namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public interface IDynamic
{
    /// <summary>
    /// Values on this game object which can change over time.
    /// </summary>
    IEnumerable<object?> DynamicValues { get; }

    /// <summary>
    /// Serializes this object when it changes so it can be sent to the client
    /// </summary>
    string Serialize(SerializationOptions options);
}