namespace GearBox.Core.Model.Static;

using GearBox.Core.Model;

public class StaticWorldContent : ISerializable<StaticWorldContentJson>
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


    public StaticWorldContentJson ToJson()
    {
        return new StaticWorldContentJson()
        {
            Map = Map.ToJson(),
            GameObjects = GameObjects.Select(x => x.ToJson())
        };
    }
}