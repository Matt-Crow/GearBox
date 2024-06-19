using GearBox.Core.Model.Json.WorldInit;

namespace GearBox.Core.Model.Json;

public readonly struct MapJson : IJson
{
    public MapJson(
        int width,
        int height,
        List<TileSetJson> pits,
        List<TileSetJson> notPits)
    {
        Width = width;
        Height = height;
        Pits = pits;
        NotPits = notPits;
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public List<TileSetJson> Pits { get; init; }
    public List<TileSetJson> NotPits { get; init; }
}