using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// An object in a world which updates every game tick
/// </summary>
public interface IDynamicGameObject : IGameObject, ISerializable<IDynamicGameObjectJson>
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

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    void Update();
}