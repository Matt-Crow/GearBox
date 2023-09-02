namespace GearBox.Core.Model;

public readonly struct Color : ISerializable<ColorJson>
{
    public static readonly Color RED = new(255, 0, 0);
    public static readonly Color GREEN = new(0, 255, 0);
    public static readonly Color BLUE = new(0, 0, 255);

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
        throw new NotImplementedException();
    }
}