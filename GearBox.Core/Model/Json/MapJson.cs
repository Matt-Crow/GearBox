using GearBox.Core.Model.Json.WorldInit;

namespace GearBox.Core.Model.Json;

public readonly struct MapJson : IJson
{
    public MapJson(
        int width,
        int height,
        List<TileJson> pits,
        List<TileJson> floors,
        List<TileJson> walls)
    {
        Width = width;
        Height = height;
        Pits = pits;
        Floors = floors;
        Walls = walls;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public List<TileJson> Pits { get; init; }
    public List<TileJson> Floors { get; init; }
    public List<TileJson> Walls { get; init; }
}