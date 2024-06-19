namespace GearBox.Core.Model.Json;

public readonly struct CoordinateJson
{
    public CoordinateJson(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; init; }
    public int Y { get; init; }
}