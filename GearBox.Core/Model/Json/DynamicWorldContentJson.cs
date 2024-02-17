namespace GearBox.Core.Model.Json;

public readonly struct DynamicWorldContentJson : IJson
{
    public DynamicWorldContentJson(List<IDynamicGameObjectJson> gameObjects)
    {
        GameObjects = gameObjects;
    }

    public List<IDynamicGameObjectJson> GameObjects { get; init; }
}