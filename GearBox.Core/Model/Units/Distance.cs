namespace GearBox.Core.Model.Units;

public readonly struct Distance
{
    private readonly int _inPixels;
    private static readonly int PIXELS_PER_TILE = 100;
    public static readonly Distance NONE = Distance.FromPixels(0);
    
    private Distance(int inPixels)
    {
        _inPixels = inPixels;
    }

    public static Distance FromPixels(int inPixels)
    {
        return new Distance(inPixels);
    }

    public static Distance FromTiles(int tiles)
    {
        return new Distance(tiles * PIXELS_PER_TILE);
    }

    public int InPixels { get => _inPixels; }
    public int InTiles { get => _inPixels / PIXELS_PER_TILE; }

    public override string ToString()
    {
        return _inPixels.ToString();
    }
}