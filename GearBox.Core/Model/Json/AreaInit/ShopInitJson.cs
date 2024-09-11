namespace GearBox.Core.Model.Json.AreaInit;

public readonly struct ShopInitJson
{
    public ShopInitJson(string name, int xInPixels, int yInPixels, ColorJson color)
    {
        Name = name;
        XInPixels = xInPixels;
        YInPixels = yInPixels;
        Color = color;
    }

    public string Name { get; init; }
    public int XInPixels { get; init; }
    public int YInPixels { get; init; }
    public ColorJson Color { get; init; }
}