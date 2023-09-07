namespace GearBox.Core.Model.Dynamic;

public readonly struct CharacterJson : IDynamicGameObjectJson
{
    public CharacterJson(Guid id, int xInPixels, int yInPixels)
    {
        Id = id;
        X = xInPixels;
        Y = yInPixels;
    }

    public Guid Id { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}