using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.Items.Crafting;

public class Crafter
{
    private readonly IItemFactory _items;
    private readonly CraftingRecipeRepository _craftingRecipes;

    public Crafter(IItemFactory items, CraftingRecipeRepository craftingRecipes)
    {
        _items = items;
        _craftingRecipes = craftingRecipes;
    }

    public void Craft(Guid craftingRecipeId, Inventory inventory)
    {
        var craftingRecipe = _craftingRecipes.GetById(craftingRecipeId);
        if (craftingRecipe == null)
        {
            return;
        }

        var ingredients = craftingRecipe.Ingredients
            .Select(dto => new ItemStack<Material>(_items.MakeMaterial(dto.ItemName), dto.Quantity));
        var canBeCrafted = ingredients.All(ingredient => inventory.Materials.Contains(ingredient.Item, ingredient.Quantity));
        if (!canBeCrafted)
        {
            return;
        }

        foreach (var ingredient in ingredients)
        {
            inventory.Materials.Remove(ingredient.Item, ingredient.Quantity);
        }

        /*
            Craft the item at level 1.
            This prevents players from getting overleveled items in low level areas
        */
        var maybeItem = _items.Make(craftingRecipe.ResultItemName);
        inventory.Add(maybeItem);
    }
}