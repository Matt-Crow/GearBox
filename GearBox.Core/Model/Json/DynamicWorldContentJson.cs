namespace GearBox.Core.Model.Json;

public readonly struct DynamicWorldContentJson : IJson
{
    public DynamicWorldContentJson(List<GameObjectJson> gameObjects)
    {
        GameObjects = gameObjects;
    }

    public List<GameObjectJson> GameObjects { get; init; }
}