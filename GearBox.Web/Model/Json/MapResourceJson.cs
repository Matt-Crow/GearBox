using GearBox.Core.Model;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Web.Model.Json;

/// <summary>
/// Represents a map from the maps folder.
/// </summary>
public class MapResourceJson
{
    public List<List<int>> Tiles { get; set; } = [];
    public List<TileTypeResourceJson> TileTypes { get; set; } = [];

    /// <summary>
    /// Attempts to convert from JSON to a map.
    /// Throws an exception if the JSON is not formatted properly.
    /// </summary>
    public Map ToMap(IRandomNumberGenerator rng)
    {
        var width = Tiles.Max(row => row.Count);
        var result = new Map(new Dimensions(
            Distance.FromTiles(Tiles.Count), 
            Distance.FromTiles(width)
        ), rng);

        foreach (var tileType in TileTypes)
        {
            var color = Color.FromName(tileType.ColorName) ?? throw new ArgumentException($"Invalid color name: {tileType.ColorName}");
            var height = TileHeight.FromName(tileType.HeightName) ?? throw new ArgumentException($"Invalid height name: {tileType.HeightName}");
            result.SetTileTypeForKey(tileType.Key, new(color, height));
        }

        result.SetTilesFrom(Tiles);
        return result;
    }
}