namespace GearBox.Core.Model.Json.AreaUpdate;

/// <summary>
/// Notifies clients of changes to the area they're connected to.
/// Each instance is intended for a single recipient.
/// </summary>
public struct AreaUpdateJson : IJson
{
    public AreaUpdateJson(List<GameObjectJson> gameObjects)
    {
        GameObjects = gameObjects;
    }

    /// <summary>
    /// All the game objects currently in the area
    /// </summary>
    public List<GameObjectJson> GameObjects { get; init; }

    public UiStateChangesJson? UiStateChanges { get; set; }
}