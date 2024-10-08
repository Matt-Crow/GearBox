namespace GearBox.Core.Model.Json.GameInit;

/// <summary>
/// Sent to a user when they first connect to a server.
/// Contains immutable data relevant to all areas of a game.
/// </summary>
public readonly struct GameInitJson : IJson
{
    public GameInitJson(Guid playerId, List<CraftingRecipeJson> craftingRecipes)
    {
        PlayerId = playerId;
        CraftingRecipes = craftingRecipes;
    }

    public Guid PlayerId { get; init; }
    public List<CraftingRecipeJson> CraftingRecipes { get; init; }
}