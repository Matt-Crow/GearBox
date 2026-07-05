using GearBox.Core.Model.Json.GameInit;

namespace GearBox.Core.Model.Items.Crafting;

/// <summary>
/// The raw data about a CraftingRecipe
/// </summary>
public class CraftingRecipe
{
    public CraftingRecipe(List<ItemStack<Material>> ingredients, ItemUnion makes, Guid? id = null)
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
            .GroupBy(stack => stack.Item)
            .Select(group => new ItemStack<Material>(group.Key, group.Sum(stack => stack.Quantity)))
            .ToList();
        
        Makes = makes;
    }


    public Guid Id { get; init; }
    public List<ItemStack<Material>> Ingredients { get; init; }
    public ItemUnion Makes { get; init; }


    public CraftingRecipeJson ToJson()
    {
        var ingredients = Ingredients
            .Select(stack => stack.ToJson())
            .ToList();
        var makes = Makes.ToJson();
        var result = new CraftingRecipeJson(Id, ingredients, makes);
        return result;
    }
}