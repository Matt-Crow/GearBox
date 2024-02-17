namespace GearBox.Core.Model.Json;

public readonly struct MapJson : IJson
{
    public MapJson(List<List<int>> tileMap, List<KeyValueJson<int, TileTypeJson>> tileTypes)
    {
        TileMap = tileMap;
        TileTypes = tileTypes;
    }

    public List<List<int>> TileMap { get; init; }
    public List<KeyValueJson<int, TileTypeJson>> TileTypes { get; init; }
}