namespace GearBox.Core.Model.GameObjects;

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
    /// If set, this determines when the object is terminated,
    /// and thus should no longer be used.
    /// </summary>
    TerminateBehavior? Termination { get; }
}