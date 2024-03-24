namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// An object in a world which updates every game tick
/// </summary>
public interface IDynamicGameObject : IGameObject
{
    /// <summary>
    /// The physical body of this game object.
    /// This object may not have a body.
    /// </summary>
    BodyBehavior? Body { get; }


    /// <summary>
    /// Whether this object has terminated,
    /// and thus should no longer be used.
    /// </summary>
    bool IsTerminated { get; }
}