namespace GearBox.Core.Model.Json;

public readonly struct ProjectileJson : IDynamicGameObjectJson
{
    public ProjectileJson(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }
    // todo add sprite later
}