namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct ProjectileJson : IJson
{
    public ProjectileJson(int x, int y, int radius, int bearingInDegrees, ColorJson color)
    {
        X = x;
        Y = y;
        Radius = radius;
        BearingInDegrees = bearingInDegrees;
        Color = color;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public int Radius { get; init; }
    public int BearingInDegrees { get; init; }
    public ColorJson Color { get; init; }
}