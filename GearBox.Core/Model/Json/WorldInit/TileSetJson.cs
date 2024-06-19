namespace GearBox.Core.Model.Json.WorldInit;

public readonly struct TileSetJson
{
    public TileSetJson(TileTypeJson tileType, List<CoordinateJson> coordinates)
    {
        TileType = tileType;
        Coordinates = coordinates;
    }

    public TileTypeJson TileType { get; init; }
    public List<CoordinateJson> Coordinates { get; init; }
}