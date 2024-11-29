namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct CharacterJson : IJson
{
    public CharacterJson(Guid id, string name, int level, ColorJson color, int xInPixels, int yInPixels, FractionJson hitPoints)
    {
        Id = id;
        Name = name;
        Level = level;
        Color = color;
        X = xInPixels;
        Y = yInPixels;
        HitPoints = hitPoints;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Level { get; init; }
    public ColorJson Color { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public FractionJson HitPoints { get; init; }
}