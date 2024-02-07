using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Static;

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
        var map = Map.ToJson();
        var gameObjects = GameObjects
            .Select(x => x.ToJson())
            .ToList();
        return new StaticWorldContentJson(map, gameObjects);
    }
}