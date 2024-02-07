namespace GearBox.Core.Model.Json;

public readonly struct StaticWorldContentJson : IJson
{
    public StaticWorldContentJson(MapJson map, List<IJson> gameObjects)
    {
        Map = map;
        GameObjects = gameObjects;
    }

    public MapJson? Map { get; init; }
    public List<IJson> GameObjects { get; init; }
}