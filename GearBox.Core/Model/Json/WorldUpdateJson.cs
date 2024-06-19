namespace GearBox.Core.Model.Json;

/// <summary>
/// Notifies clients of changes to the world they're connected to.
/// Each WorldUpdateJson is intended for a single recipient.
/// </summary>
public readonly struct WorldUpdateJson : IJson
{
    public WorldUpdateJson(List<GameObjectJson> gameObjects, StableJson inventory, StableJson weapon, StableJson armor, StableJson statSummary)
    {
        GameObjects = gameObjects;
        Inventory = inventory;
        Weapon = weapon;
        Armor = armor;
        StatSummary = statSummary;
    }
    
    /// <summary>
    /// All the game objects currently in the world
    /// </summary>
    public List<GameObjectJson> GameObjects { get; init; }

    /// <summary>
    /// Changes to the recipient's inventory
    /// </summary>
    public StableJson Inventory { get; init; }
    
    /// <summary>
    /// Changes to the recipient's equipped weapon
    /// </summary>
    public StableJson Weapon { get; init; }

    /// <summary>
    /// Changes to the recipient's equipped armor
    /// </summary>
    public StableJson Armor { get; init; }

    /// <summary>
    /// Changes to the recipient's stats
    /// </summary>
    public StableJson StatSummary { get; init; }
}