using GearBox.Core.Model.Json.WorldInit;

namespace GearBox.Core.Model.Json;

public readonly struct MapJson : IJson
{
    public MapJson(
        int width,
        int height,
        List<TileSetJson> pits,
        List<TileSetJson> floors,
        List<TileSetJson> walls)
    {
        Width = width;
        Height = height;
        Pits = pits;
        Floors = floors;
        Walls = walls;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public List<TileSetJson> Pits { get; init; }
    public List<TileSetJson> Floors { get; init; }
    public List<TileSetJson> Walls { get; init; }
}