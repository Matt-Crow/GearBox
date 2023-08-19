namespace GearBox.Core.Model.Units;

public readonly struct Dimensions
{
    private readonly Distance _width;
    private readonly Distance _height;


    private Dimensions(Distance width, Distance height)
    {
        _width = width;
        _height = height;
    }

    public static Dimensions InTiles(int length)
    {
        if (length < 0)
        {
            throw new ArgumentException("length must be non-negative");
        }
        var distance = Distance.FromTiles(length);
        return new Dimensions(distance, distance);
    }


    public int WidthInPixels { get => _width.InPixels; }
    public int WidthInTiles { get => _width.InTiles; }
    public int HeightInPixels { get => _height.InPixels; }
    public int HeightInTiles { get => _height.InTiles; }
}