namespace GearBox.Core.Model.Json;

public readonly struct CharacterJson : IDynamicGameObjectJson
{
    public CharacterJson(Guid id, int xInPixels, int yInPixels, FractionJson hitPoints)
    {
        Id = id;
        X = xInPixels;
        Y = yInPixels;
        HitPoints = hitPoints;
    }

    public Guid Id { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public FractionJson HitPoints { get; init; }
}