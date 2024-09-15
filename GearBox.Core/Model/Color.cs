using GearBox.Core.Model.Json;

namespace GearBox.Core.Model;

public readonly struct Color : ISerializable<ColorJson>
{
    public static readonly Color RED = new("red", 255, 0, 0);
    public static readonly Color GREEN = new("green", 0, 255, 0);
    public static readonly Color LIGHT_GREEN = new("light green", 0, 100, 0);
    public static readonly Color BLUE = new("blue", 0, 0, 255);
    public static readonly Color BLACK = new("black", 0, 0, 0);
    public static readonly Color GRAY = new("gray", 128, 128, 128);
    public static readonly Color TAN = new("tan", 200, 180, 140);
    public static readonly IEnumerable<Color> ALL = [
        RED, 
        GREEN,
        LIGHT_GREEN,
        BLUE,
        BLACK,
        GRAY,
        TAN
    ];

    private Color(string name, int r, int g, int b)
    {
        Name = name.ToLower();
        Red = r;
        Green = g;
        Blue = b;
    }

    public static Color? FromName(string name)
    {
        name = name.ToLower();
        return ALL.FirstOrDefault(x => x.Name == name);
    }

    public string Name { get; init; }
    public int Red { get; init; }
    public int Green { get; init; }
    public int Blue { get; init; }

    public ColorJson ToJson()
    {
        return new ColorJson(Red, Green, Blue);
    }
}