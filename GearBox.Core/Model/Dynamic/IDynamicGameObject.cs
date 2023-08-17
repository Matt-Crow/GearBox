namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// An object in a world which updates every game tick
/// </summary>
public interface IDynamicGameObject : IGameObject
{
    /// <summary>
    /// Called each game tick.
    /// Subclasses should not call this method.
    /// </summary>
    public void Update();
}