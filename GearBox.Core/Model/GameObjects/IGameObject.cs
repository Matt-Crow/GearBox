namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// An object in a world which updates every game tick
/// </summary>
public interface IGameObject
{
    /// <summary>
    /// The physical body of this game object.
    /// This object may not have a body.
    /// </summary>
    BodyBehavior? Body { get; }

    /// <summary>
    /// If specified, this serializer will be used to serialize this object and send it to the front end.
    /// If not specified, this object will never be sent to the front end.
    /// </summary>
    Serializer? Serializer { get; }

    /// <summary>
    /// If set, this determines when the object is terminated,
    /// and thus should no longer be used.
    /// </summary>
    TerminateBehavior? Termination { get; }

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    void Update();
}