namespace GearBox.Core.Model.Json.AreaInit;

/// <summary>
/// Sent as a message to users upon first connecting to an area.
/// It contains all information which will never change, and thus only needs to
/// be sent once.
/// </summary>
public readonly struct AreaInitJson : IJson
{
    public AreaInitJson(Guid playerId, MapJson map, List<ItemTypeJson> itemTypes, List<CraftingRecipeJson> craftingRecipes)
    {
        PlayerId = playerId;
        Map = map;
        ItemTypes = itemTypes;
        CraftingRecipes = craftingRecipes;
    }

    /// <summary>
    /// The Id of the character spawned into the area for the user.
    /// </summary>
    public Guid PlayerId { get; init; }

    public MapJson Map { get; init; }

    /// <summary>
    /// All the possible item types which can exist in the area.
    /// </summary>
    public List<ItemTypeJson> ItemTypes { get; init; }

    /// <summary>
    /// All the possible recipes players can craft
    /// </summary>
    public List<CraftingRecipeJson> CraftingRecipes { get; init; }
}