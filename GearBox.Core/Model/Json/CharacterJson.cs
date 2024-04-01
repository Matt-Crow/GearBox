namespace GearBox.Core.Model.Json;

public readonly struct CharacterJson : IJson
{
    public CharacterJson(Guid id, string name, int level, int xInPixels, int yInPixels, FractionJson hitPoints)
    {
        Id = id;
        Name = name;
        Level = level;
        X = xInPixels;
        Y = yInPixels;
        HitPoints = hitPoints;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Level { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public FractionJson HitPoints { get; init; }
}