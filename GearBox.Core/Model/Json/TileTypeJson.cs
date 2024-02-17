namespace GearBox.Core.Model.Json;

public readonly struct TileTypeJson : IJson
{
    public TileTypeJson(ColorJson color, bool isTangible)
    {
        Color = color;
        IsTangible = isTangible;
    }
    
    public ColorJson Color { get; init; }
    public bool IsTangible { get; init; }
}