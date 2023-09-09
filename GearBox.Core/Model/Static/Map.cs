namespace GearBox.Core.Model.Static;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Units;

/// <summary>
/// a Map is a matrix of tiles the player can see and collide with
/// </summary>
public class Map : ISerializable<MapJson>
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
        _tileTypes[0] = TileType.Intangible(Color.BLUE);
    }


    public Distance Width { get => Distance.FromTiles(_tileMap.GetLength(1)); }
    public Distance Height { get => Distance.FromTiles(_tileMap.GetLength(0)); }


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

    public bool IsValid(Coordinates coordinates)
    {
        // do not use try/catch around Validate, significant performance issues
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        if (0 > x || x >= Width.InTiles)
        {
            return false;
        }
        if (0 > y || y >= Height.InTiles)
        {
            return false;
        }
        return true;
    }

    private void Validate(Coordinates coordinates)
    {
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        if (0 > x || x >= Width.InTiles)
        {
            throw new ArgumentException($"require 0 <= x < {Width.InTiles}");
        }
        if (0 > y || y >= Height.InTiles)
        {
            throw new ArgumentException($"require 0 <= y < {Height.InTiles}");
        }
    }

    /// <summary>
    /// Checks if the given object collides with any part of the map,
    /// then moves it if needed.
    /// </summary>
    /// <param name="body">the object to check for collisions with</param>
    public void CheckForCollisions(BodyBehavior body)
    {
        KeepInBounds(body);
        ShoveOutOfTiles(body);
    }

    private void KeepInBounds(BodyBehavior body)
    {
        if (body.LeftInPixels < 0)
        {
            body.LeftInPixels = 0;
        }
        if (body.RightInPixels > Width.InPixels)
        {
            body.RightInPixels = Width.InPixels;
        }
        if (body.TopInPixels < 0)
        {
            body.TopInPixels = 0;
        }
        if (body.BottomInPixels > Height.InPixels)
        {
            body.BottomInPixels = Height.InPixels;
        }
    }

    private void ShoveOutOfTiles(BodyBehavior body)
    {
        if (body.Radius.InPixels * 2 > Distance.FromTiles(1).InPixels)
        {
            throw new Exception("Objects with large radius are not supported yet");
        }

        // For now, we're only dealing with bodies with radius of 1/2 tile or less.
        // That makes the math a lot easier.

        /*
            Since this body is 1x1 tiles at most, it can occupy at most 4 times,
            like so
                aabb
                axxb
                cxxd
                ccdd
            Where a, b, c, and d are tiles, and x is the body.
            So let's start checking in their upper-left corner, then check their
            other corners.
        */
        var upperLeft = Coordinates.FromPixels(body.LeftInPixels, body.TopInPixels);
        // truncate to upper left tile
        var upperLeftTile = Coordinates.FromTiles(upperLeft.XInTiles, upperLeft.YInTiles);

        // check all the tiles they occupy
        for (var iter = new TileIterator(this, upperLeftTile, Dimensions.InTiles(2)); !iter.Done; iter.Next())
        {
            var tile = iter.Current;
            if (tile.TileType.IsTangible && tile.IsCollidingWith(body))
            {
                tile.ShoveOut(body);
            }
        }
    }

    public MapJson ToJson()
    {
        // Convert 2d array to 2d list: https://stackoverflow.com/a/37458182/11110116
        var tm = _tileMap.Cast<int>() 
            .Select((x,i)=> new {x, index = i/_tileMap.GetLength(1)})
            .GroupBy(x=>x.index)
            .Select(x=>x.Select(s=>s.x).ToList())  
            .ToList();

        var tt = new List<KeyValueJson<int, TileTypeJson>>();
        foreach (var kv in _tileTypes)
        {
            tt.Add(new KeyValueJson<int, TileTypeJson>(kv.Key, kv.Value.ToJson()));
        }

        return new MapJson(tm, tt);
    }
}