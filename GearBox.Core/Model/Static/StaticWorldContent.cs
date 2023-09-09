namespace GearBox.Core.Model.Static;

using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;

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

    /// <summary>
    /// Checks if the given object collides with any part of the map,
    /// then moves it if needed.
    /// </summary>
    /// <param name="body">the object to check for collisions with</param>
    public void CheckForCollisions(BodyBehavior body)
    {
        Map.CheckForCollisions(body);
        // GameObjects may one day check for collisions too
    }

    public StaticWorldContentJson ToJson()
    {
        return new StaticWorldContentJson()
        {
            Map = Map.ToJson(),
            GameObjects = GameObjects.Select(x => x.ToJson())
        };
    }
}