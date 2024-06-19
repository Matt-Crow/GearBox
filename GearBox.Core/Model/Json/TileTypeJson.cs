namespace GearBox.Core.Model.Json;

public readonly struct TileTypeJson : IJson
{
    public TileTypeJson(ColorJson color, int height)
    {
        Color = color;
        Height = height;
    }
    
    public ColorJson Color { get; init; }
    public int Height { get; init; }
}