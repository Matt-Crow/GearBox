namespace GearBox.Core.Model.Items.Crafting;

/// <summary>
/// The raw data about a CraftingRecipe
/// </summary>
public class CraftingRecipeDTO
{
    public CraftingRecipeDTO(List<ItemStackDTO> ingredients, string resultItemName, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Ingredients = ingredients
            .GroupBy(i => i.ItemName)
            .Select(group => new ItemStackDTO(group.Key, group.Sum(stack => stack.Quantity)))
            .ToList();
        ResultItemName = resultItemName;
    }

    public Guid Id { get; init; }
    public List<ItemStackDTO> Ingredients { get; init; }
    public string ResultItemName { get; init; }
}