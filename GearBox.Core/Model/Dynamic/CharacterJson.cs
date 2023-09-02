namespace GearBox.Core.Model.Dynamic;

public readonly struct CharacterJson : IDynamicGameObjectJson
{
    public CharacterJson(int xInPixels, int yInPixels)
    {
        XInPixels = xInPixels;
        YInPixels = yInPixels;
    }

    public int XInPixels { get; init; }
    public int YInPixels { get; init; }
}