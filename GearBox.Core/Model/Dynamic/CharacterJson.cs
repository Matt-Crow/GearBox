namespace GearBox.Core.Model.Dynamic;

public readonly struct CharacterJson : IDynamicGameObjectJson
{
    public CharacterJson(int xInPixels, int yInPixels)
    {
        X = xInPixels;
        Y = yInPixels;
    }

    public int X { get; init; }
    public int Y { get; init; }
}