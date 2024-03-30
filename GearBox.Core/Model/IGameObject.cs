namespace GearBox.Core.Model;

public interface IGameObject
{
    /// <summary>
    /// If specified, this serializer will be used to serialize this object and send it to the front end.
    /// If not specified, this object will never be sent to the front end.
    /// </summary>
    Serializer? Serializer { get; }

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    void Update();
}