namespace GearBox.Core.Model.Json.AreaUpdate;

/// <summary>
/// Notifies clients of changes to the area they're connected to.
/// Each WorldUpdateJson is intended for a single recipient.
/// </summary>
public readonly struct AreaUpdateJson : IJson
{
    public AreaUpdateJson(List<GameObjectJson> gameObjects, ChangesJson changes)
    {
        GameObjects = gameObjects;
        Changes = changes;
    }

    /// <summary>
    /// All the game objects currently in the world
    /// </summary>
    public List<GameObjectJson> GameObjects { get; init; }

    public ChangesJson Changes { get; init; }
}