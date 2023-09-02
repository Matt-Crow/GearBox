namespace GearBox.Core.Model.Static;

public readonly struct TileTypeJson : IJson
{
    public TileTypeJson(Color color, bool isTangible)
    {
        Color = color;
        IsTangible = isTangible;
    }
    
    public Color Color { get; init; }
    public bool IsTangible { get; init; }
}