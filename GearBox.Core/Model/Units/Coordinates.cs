namespace GearBox.Core.Model.Units;

public struct Coordinates
{
    private double _x;
    private double _y;


    private Coordinates(double x, double y)
    {
        _x = x;
        _y = y;
    }


    public static Coordinates ORIGIN { get; } = new Coordinates(0, 0);


    public static Coordinates InPixels(double x, double y)
    {
        return new Coordinates(x, y);
    }

    public static Coordinates InTiles(double x, double y)
    {
        return InPixels(x * Tile.SIZE, y * Tile.SIZE);
    }


    public double XInPixels { get => _x; }
    public int XInTiles { get => (int)(_x / Tile.SIZE); }
    
    public double YInPixels { get => _y; }
    public int YInTiles { get => (int)(_y / Tile.SIZE); }

    public Coordinates Plus(Velocity v)
    {
        return Coordinates.InPixels(
            XInPixels + v.ChangeInXInPixels,
            YInPixels + v.ChangeInYInPixels
        );
    }

    public override string ToString()
    {
        return $"({_x}, {_y})";
    }
}