namespace GearBox.Core.Model.Json.AreaInit;

public readonly struct TileJson
{
    public TileJson(ColorJson color, int x, int y)
    {
        Color = color;
        X = x;
        Y = y;
    }

    public ColorJson Color { get; init; }
    public int X { get; init; } 
    public int Y { get; init; }
}