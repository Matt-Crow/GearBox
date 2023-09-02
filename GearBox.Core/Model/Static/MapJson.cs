namespace GearBox.Core.Model.Static;

public class MapJson : IJson
{
    public MapJson(List<List<int>> tileMap, List<KeyValueJson<int, TileTypeJson>> tileTypes)
    {
        TileMap = tileMap;
        TileTypes = tileTypes;
    }

    public List<List<int>> TileMap { get; set; }
    public List<KeyValueJson<int, TileTypeJson>> TileTypes { get; init; }
}