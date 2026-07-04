namespace GearBox.Core.Model.Items.Crafting;

/// <summary>
/// The raw data about a CraftingRecipe
/// </summary>
public class CraftingRecipeDTO
{
    public CraftingRecipeDTO(List<ItemStackDTO> ingredients, string resultItemName, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();

        /* 
            group stacks by item name to remove duplicates

            Before:
                Apple x1
                Bananas x2
                Bananas x3
            
            After:
                Apple x1
                Bananas x5
        */
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