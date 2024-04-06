namespace GearBox.Core.Model.Stable;

/// <summary>
/// A stable game object is one which changes infrequently, and thus only needs 
/// to be synced with the clients occasionally.
/// </summary>
public interface IStableGameObject : IDynamic, IGameObject
{

}