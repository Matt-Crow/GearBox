using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Json.AreaInit;

public readonly struct ShopInitJson
{
    public ShopInitJson(string name, int xInPixels, int yInPixels, ColorJson color, InventoryJson options)
    {
        Name = name;
        XInPixels = xInPixels;
        YInPixels = yInPixels;
        Color = color;
        Options = options;
    }

    public string Name { get; init; }
    public int XInPixels { get; init; }
    public int YInPixels { get; init; }
    public ColorJson Color { get; init; }
    public InventoryJson Options { get; init; }
}