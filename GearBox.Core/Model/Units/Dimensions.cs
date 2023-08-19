namespace GearBox.Core.Model.Units;

public readonly struct Dimensions
{
    private readonly int _width;
    private readonly int _height;


    private Dimensions(int width, int height)
    {
        if (width < 0)
        {
            throw new ArgumentException("width must be at least 0");
        }
        if (height < 0)
        {
            throw new ArgumentException("height must be at least 0");
        }
        _width = width;
        _height = height;
    }

    public static Dimensions InTiles(int length)
    {
        return new Dimensions(length * Tile.SIZE, length * Tile.SIZE);
    }


    public int WidthInPixels { get => _width; }
    public int WidthInTiles { get => _width / Tile.SIZE; }
    public int HeightInPixels { get => _height; }
    public int HeightInTiles { get => _height / Tile.SIZE; }
}