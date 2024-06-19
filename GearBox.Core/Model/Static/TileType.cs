namespace GearBox.Core.Model.Static;

/// <summary>
/// defines one type of tile which may exist on a map
/// </summary>
public class TileType
{
    public TileType(Color color, TileHeight height)
    {
        Color = color;
        Height = height;
    }

    public Color Color { get; init; }
    public TileHeight Height { get; init; }
}