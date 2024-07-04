namespace GearBox.Core.Model.Json.GameInit;

public readonly struct CraftingRecipeJson : IJson
{
    public CraftingRecipeJson(Guid id, List<ItemJson> ingredients, ItemJson makes)
    {
        Id = id;
        Ingredients = ingredients;
        Makes = makes;
    }

    public Guid Id { get; init; }
    public List<ItemJson> Ingredients { get; init; }
    public ItemJson Makes { get; init; }
}