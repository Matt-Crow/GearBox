namespace GearBox.Core.Model.Items.Crafting;

/// <summary>
/// The raw data about a CraftingRecipe
/// </summary>
public class CraftingRecipeDTO
{
    public CraftingRecipeDTO(List<ItemStackDTO> ingredients, string resultItemName)
    {
        Ingredients = ingredients;
        ResultItemName = resultItemName;
    }

    public List<ItemStackDTO> Ingredients { get; init; }
    public string ResultItemName { get; init; }
}