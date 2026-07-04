namespace GearBox.Core.Model.Items.Crafting;

public class Crafter
{
    private readonly CraftingRecipeRepository _craftingRecipes;

    public Crafter(CraftingRecipeRepository craftingRecipes)
    {
        _craftingRecipes = craftingRecipes;
    }

    public void Craft(Guid craftingRecipeId, Inventory inventory)
    {
        var craftingRecipe = _craftingRecipes.GetById(craftingRecipeId);
        if (craftingRecipe == null)
        {
            return;
        }

        var canBeCrafted = craftingRecipe.Ingredients.All(ingredient => inventory.Materials.Contains(ingredient.Item, ingredient.Quantity));
        if (!canBeCrafted)
        {
            return;
        }

        foreach (var ingredient in craftingRecipe.Ingredients)
        {
            inventory.Materials.Remove(ingredient.Item, ingredient.Quantity);
        }

        /*
            Craft the item at level 1.
            This prevents players from getting overleveled items in low level areas
        */
        var item = craftingRecipe.Maker.Invoke();
        inventory.Add(item);
    }
}