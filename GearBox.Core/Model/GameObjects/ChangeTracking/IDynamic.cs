namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public interface IDynamic<TJson> : IMightChange<TJson>
{
    /// <summary>
    /// Serializes this object when it changes so it can be sent to the client
    /// </summary>
    string Serialize(SerializationOptions options);
}