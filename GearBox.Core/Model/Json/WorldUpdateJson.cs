namespace GearBox.Core.Model.Json;

/// <summary>
/// Notifies clients of changes to the world they're connected to
/// </summary>
public readonly struct WorldUpdateJson : IJson
{
    public WorldUpdateJson(List<GameObjectJson> gameObjects)
    {
        GameObjects = gameObjects;
    }
    
    public List<GameObjectJson> GameObjects { get; init; }
}