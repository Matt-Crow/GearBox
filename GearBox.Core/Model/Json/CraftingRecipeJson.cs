namespace GearBox.Core.Model.Json;

public readonly struct CraftingRecipeJson : IJson
{
    public CraftingRecipeJson(List<ItemJson> ingredients, ItemJson makes)
    {
        Ingredients = ingredients;
        Makes = makes;
    }

    public List<ItemJson> Ingredients { get; init; }
    public ItemJson Makes { get; init; }
}