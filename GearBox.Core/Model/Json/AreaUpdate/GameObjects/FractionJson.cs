namespace GearBox.Core.Model.Json.AreaUpdate.GameObjects;

public readonly struct FractionJson : IJson
{
    public FractionJson(int current, int max)
    {
        Current = current;
        Max = max;
    }

    public int Current { get; init; }
    public int Max { get; init; }
}