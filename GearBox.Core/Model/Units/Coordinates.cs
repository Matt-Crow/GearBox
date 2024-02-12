namespace GearBox.Core.Model.Units;

public readonly struct Coordinates
{
    private readonly Distance _x;
    private readonly Distance _y;
    public static readonly Coordinates ORIGIN = new Coordinates(Distance.NONE, Distance.NONE);

    private Coordinates(Distance x2, Distance y2)
    {
        _x = x2;
        _y = y2;
    }

    public static Coordinates FromPixels(int x, int y)
    {
        return new Coordinates(Distance.FromPixels(x), Distance.FromPixels(y));
    }

    public static Coordinates FromTiles(int x, int y)
    {
        return new Coordinates(Distance.FromTiles(x), Distance.FromTiles(y));
    }

    public int XInPixels { get => _x.InPixels; }
    public int XInTiles { get => _x.InTiles; }
    
    public int YInPixels { get => _y.InPixels; }
    public int YInTiles { get => _y.InTiles; }

    public Coordinates Plus(Velocity v)
    {
        return Coordinates.FromPixels(
            (int)(XInPixels + v.ChangeInXInPixels),
            (int)(YInPixels + v.ChangeInYInPixels)
        );
    }

    public Coordinates Plus(Distance dx, Distance dy)
    {
        return new Coordinates(_x.Plus(dx), _y.Plus(dy));
    }

    public Coordinates PlusTiles(int dx, int dy)
    {
        return Coordinates.FromTiles(
            XInTiles + dx,
            YInTiles + dy
        );
    }

    public Coordinates CenteredOnTile()
    {
        return Plus(Distance.FromTiles(0.5), Distance.FromTiles(0.5));
    }

    public override string ToString()
    {
        return $"({_x}, {_y})";
    }
}