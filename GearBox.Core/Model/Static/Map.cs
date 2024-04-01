
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Static;

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


    public Distance Width => Distance.FromTiles(_tileMap.GetLength(1));
    public Distance Height => Distance.FromTiles(_tileMap.GetLength(0));


    public Map SetTileTypeForKey(int key, TileType value)
    {
        _tileTypes[key] = value;
        return this;
    }

    public Map SetTileAt(Coordinates coordinates, TileType tileType)
    {
        if (!_tileTypes.ContainsValue(tileType))
        {
            throw new ArgumentException("unsupported tile type");
        }
        var i = _tileTypes.FirstOrDefault(x => x.Value == tileType).Key;
        SetTileAt(coordinates, i);
        return this;
    }

    public Map SetTileAt(Coordinates coordinates, int tileType)
    {
        Validate(coordinates);
        var x = coordinates.XInTiles;
        var y = coordinates.YInTiles;
        if (!_tileTypes.ContainsKey(tileType))
        {
            throw new ArgumentException($"invalid tileType {tileType}");
        }
        _tileMap[y,x] = tileType;
        return this;
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

    public Coordinates? GetRandomOpenTile()
    {
        var random = new Random();
        var x = random.Next(Width.InTiles);
        var y = random.Next(Height.InTiles);
        var source = Coordinates.FromTiles(x, y);
        return GetOpenTileAround(source);
    }

    private Coordinates? GetOpenTileAround(Coordinates source, int searchRadius=0)
    {
        /*
            recursively search squares of tiles around a source

            searchRadius = 0
                S
            
            searchRadius = 1
                XXX
                X X
                XXX

            searchRadius = 2
                XXXXX
                X   X
                X   X
                X   X
                XXXXX
        */
        var minX = source.XInTiles - searchRadius;
        var maxX = source.XInTiles + searchRadius;
        var minY = source.YInTiles - searchRadius;
        var maxY = source.YInTiles + searchRadius;

        var upperL = Coordinates.FromTiles(minX, minY);
        var upperR = Coordinates.FromTiles(maxX, minY);
        var lowerR = Coordinates.FromTiles(maxX, maxY);
        var lowerL = Coordinates.FromTiles(minX, maxY);

        // base case: search radius is too large to find anything
        if (!IsValid(upperL) && !IsValid(upperR) && !IsValid(lowerR) && !IsValid(lowerL))
        {
            return null;
        }
        
        Coordinates? found = null;
        found ??= SearchForOpenTileAlongLine(upperL, upperR, 1, 0); // right across the top
        found ??= SearchForOpenTileAlongLine(upperR, lowerR, 0, 1); // down the right
        found ??= SearchForOpenTileAlongLine(lowerR, lowerL, -1, 0); // left across the bottom
        found ??= SearchForOpenTileAlongLine(lowerL, upperL, 0, -1); // up the left

        return found ?? GetOpenTileAround(source, searchRadius + 1);
    }

    private Coordinates? SearchForOpenTileAlongLine(Coordinates start, Coordinates end, int dx, int dy)
    {
        for (var curr = start; !curr.Equals(end); curr = curr.PlusTiles(dx, dy))
        {
            if (IsValid(curr) && !GetTileAt(curr).IsTangible)
            {
                return curr;
            }
        }
        return null;
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