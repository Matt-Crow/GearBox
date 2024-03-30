namespace GearBox.Core.Model.Json;

public readonly struct PlayerJson : IJson
{
    public PlayerJson(Guid id, int x, int y, FractionJson hitPoints, FractionJson energy)
    {
        Id = id;
        X = x;
        Y = y;
        HitPoints = hitPoints;
        Energy = energy;
    }

    public Guid Id { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public FractionJson HitPoints { get; init; }
    public FractionJson Energy { get; init; }
}