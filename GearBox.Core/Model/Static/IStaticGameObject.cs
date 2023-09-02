namespace GearBox.Core.Model.Static;

/// <summary>
/// Marker interface for something which exists in a world but needn't be
/// updated after it has been initialized
/// </summary>
public interface IStaticGameObject : IGameObject, ISerializable<IJson>
{

}