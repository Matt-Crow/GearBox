using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Static;

public class StaticWorldContent : ISerializable<StaticWorldContentJson>
{
    public static readonly StaticWorldContent EMPTY = new(new Map());

    public StaticWorldContent(Map _map)
    {
        Map = _map;
    }

    public Map Map { get; init; }

    /// <summary>
    /// Checks if the given object collides with any part of the map,
    /// then moves it if needed.
    /// </summary>
    public void CheckForCollisions(BodyBehavior body) => Map.CheckForCollisions(body);

    public StaticWorldContentJson ToJson() => new(Map.ToJson());
}