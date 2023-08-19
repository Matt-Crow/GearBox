namespace GearBox.Core.Model.Static;

using GearBox.Core.Model.Units;
using System.Drawing;

/// <summary>
/// a Map is a matrix of tiles the player can see and collide with
/// </summary>
public class Map
{
    // maybe don't even need more than just tangible & intangible colors
    private readonly int[,] _tileMap;
    private readonly Dictionary<int, TileType> _tileTypes = new();


    public Map() : this(Dimensions.InTiles(20)) 
    {

    }

    public Map(Dimensions dimensions)
    {
        var width = dimensions.WidthInTiles;
        var height = dimensions.HeightInTiles;
        if (width <= 0)
        {
            throw new ArgumentException("width must be positive");
        }
        if (height <= 0)
        {
            throw new ArgumentException("height must be positive");
        }
        _tileMap = new int[height, width]; // initializes all to 0
        _tileTypes[0] = TileType.Intangible(Color.Blue);
    }


    public int Width { get => _tileMap.GetLength(1); }
    public int Height { get => _tileMap.Length; }


    public void SetTileTypeForKey(int key, TileType value)
    {
        _tileTypes[key] = value;
    }

    public void SetTileAt(Coordinates coordinates, TileType tileType)
    {
        if (!_tileTypes.ContainsValue(tileType))
        {
            throw new ArgumentException("unsupported tile type");
        }
        var i = _tileTypes.FirstOrDefault(x => x.Value == tileType).Key;
        SetTileAt(coordinates, i);
    }

    public void SetTileAt(Coordinates coordinates, int tileType)
    {
        Validate(coordinates);
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        if (!_tileTypes.ContainsKey(tileType))
        {
            throw new ArgumentException($"invalid tileType {tileType}");
        }
        _tileMap[y,x] = tileType;
    }

    public TileType GetTileAt(Coordinates coordinates)
    {
        Validate(coordinates);
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        var i = _tileMap[y,x];
        var result = _tileTypes[i];
        return result;
    }

    private void Validate(Coordinates coordinates)
    {
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        if (0 > x || x >= Width)
        {
            throw new ArgumentException($"require 0 <= x < {Width}");
        }
        if (0 > y || y >= Height)
        {
            throw new ArgumentException($"require 0 <= y < {Height}");
        }
    }
}