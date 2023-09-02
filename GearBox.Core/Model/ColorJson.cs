namespace GearBox.Core.Model;

public class ColorJson : IJson
{
    public ColorJson(int r, int g, int b)
    {
        Red = r;
        Green = g;
        Blue = b;
    }

    public int Red { get; init; }
    public int Green { get; init; }
    public int Blue { get; init; }
}