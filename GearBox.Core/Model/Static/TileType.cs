using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Static;

/// <summary>
/// defines one type of tile which may exist on a map
/// </summary>
public class TileType : ISerializable<TileTypeJson>
{
    public TileType(Color color, bool isTangible)
    {
        Color = color;
        IsTangible = isTangible;
    }

    public static TileType Tangible(Color color)
    {
        return new TileType(color, true);
    }

    public static TileType Intangible(Color color)
    {
        return new TileType(color, false);
    }

    public Color Color { get; init; }
    public bool IsTangible { get; init; }

    public TileTypeJson ToJson()
    {
        return new TileTypeJson(Color.ToJson(), IsTangible);
    }
}