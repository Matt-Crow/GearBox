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
    public BodyBehavior? Body { get; }

    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    public void Update();
}