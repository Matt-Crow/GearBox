using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.Items.Crafting;

public class Crafter
{
    private readonly Factory<CraftingRecipe> _craftingRecipes;

    public Crafter(Factory<CraftingRecipe> craftingRecipes)
    {
        _craftingRecipes = craftingRecipes;
    }

    public void Craft(Guid craftingRecipeId, Inventory inventory)
    {
        var craftingRecipe = _craftingRecipes.Make(craftingRecipeId.ToString());
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
        inventory.Add(craftingRecipe.Makes.ToOwned(1));
    }
}