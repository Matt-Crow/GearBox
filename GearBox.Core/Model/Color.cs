using GearBox.Core.Model.Json;

namespace GearBox.Core.Model;

public readonly struct Color : ISerializable<ColorJson>
{
    public static readonly Color RED = new(255, 0, 0);
    public static readonly Color GREEN = new(0, 255, 0);
    public static readonly Color LIGHT_GREEN = new(0, 100, 0);
    public static readonly Color BLUE = new(0, 0, 255);
    public static readonly Color GRAY = new(128, 128, 128);
    public static readonly Color TAN = new(200, 180, 140);

    public Color(int r, int g, int b)
    {
        Red = r;
        Green = g;
        Blue = b;
    }

    public int Red { get; init; }
    public int Green { get; init; }
    public int Blue { get; init; }

    public ColorJson ToJson()
    {
        return new ColorJson(Red, Green, Blue);
    }
}