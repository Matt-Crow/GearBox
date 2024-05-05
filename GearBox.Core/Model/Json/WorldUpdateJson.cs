namespace GearBox.Core.Model.Json;

/// <summary>
/// Notifies clients of changes to the world they're connected to.
/// Each WorldUpdateJson is intended for a single recipient.
/// </summary>
public readonly struct WorldUpdateJson : IJson
{
    public WorldUpdateJson(List<GameObjectJson> gameObjects, StableJson inventory)
    {
        GameObjects = gameObjects;
        Inventory = inventory;
    }
    
    /// <summary>
    /// All the game objects currently in the world
    /// </summary>
    public List<GameObjectJson> GameObjects { get; init; }

    /// <summary>
    /// Changes to the recipient's inventory
    /// </summary>
    public StableJson Inventory { get; init; }
}