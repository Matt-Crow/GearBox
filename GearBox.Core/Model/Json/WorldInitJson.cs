namespace GearBox.Core.Model.Json;

/// <summary>
/// Sent as a message to users upon first connecting to a world.
/// It contains all information which will never change, and thus only needs to
/// be sent once.
/// </summary>
public readonly struct WorldInitJson : IJson
{
    public WorldInitJson(Guid playerId, MapJson map, List<ItemTypeJson> itemTypes, List<CraftingRecipeJson> craftingRecipes)
    {
        PlayerId = playerId;
        Map = map;
        ItemTypes = itemTypes;
        CraftingRecipes = craftingRecipes;
    }

    /// <summary>
    /// The Id of the character spawned into the world for the user.
    /// </summary>
    public Guid PlayerId { get; init; }

    /// <summary>
    /// The contents of the world which never change.
    /// </summary>
    public MapJson Map { get; init; }

    /// <summary>
    /// All the possible item types which can exist in the world.
    /// </summary>
    public List<ItemTypeJson> ItemTypes { get; init; }

    /// <summary>
    /// All the possible recipes players can craft
    /// </summary>
    public List<CraftingRecipeJson> CraftingRecipes { get; init; }
}