namespace GearBox.Core.Model.Dynamic;

public readonly struct DynamicWorldContentJson : IJson
{
    public DynamicWorldContentJson(List<IDynamicGameObjectJson> gameObjects)
    {
        GameObjects = gameObjects;
    }

    public List<IDynamicGameObjectJson> GameObjects { get; init; }
}