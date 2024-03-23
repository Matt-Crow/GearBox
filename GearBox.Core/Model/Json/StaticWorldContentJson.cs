namespace GearBox.Core.Model.Json;

public readonly struct StaticWorldContentJson : IJson
{
    public StaticWorldContentJson(MapJson map)
    {
        Map = map;
    }

    public MapJson? Map { get; init; }
}