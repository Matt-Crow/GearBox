namespace GearBox.Core.Model.Static;

using GearBox.Core.Model;

public class StaticWorldContent : ISerializable
{
    public static readonly StaticWorldContent EMPTY = new StaticWorldContent(
        new Map(), 
        new List<IStaticGameObject>()
    );

    public StaticWorldContent(Map _map, IEnumerable<IStaticGameObject> gameObjects)
    {
        Map = _map;
        GameObjects = gameObjects;
    }

    public Map Map { get; init; }
    public IEnumerable<IStaticGameObject> GameObjects { get; init; }
}