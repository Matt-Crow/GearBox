namespace GearBox.Core.Model;

using GearBox.Core.Model.Static;

public struct Coordinates
{
    private int _x;
    private int _y;


    private Coordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }


    public static Coordinates ORIGIN { get; } = new Coordinates(0, 0);


    public static Coordinates InPixels(int x, int y)
    {
        return new Coordinates(x, y);
    }

    public static Coordinates InTiles(int x, int y)
    {
        return InPixels(x * Tile.SIZE, y * Tile.SIZE);
    }


    public int XInPixels { get => _x; }
    public int XInTiles { get => _x / Tile.SIZE; }
    
    public int YInPixels { get => _y; }
    public int YInTiles { get => _y / Tile.SIZE; }
}