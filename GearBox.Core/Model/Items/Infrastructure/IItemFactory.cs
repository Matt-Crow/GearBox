using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model.Items.Infrastructure;

/// <summary>
/// Makes items
/// </summary>
public interface IItemFactory
{
    /// <summary>
    /// Allows this to make the given item.
    /// </summary>
    IItemFactory Add(ItemUnion value);

    /// <summary>
    /// Returns a copy of the item with the given type name,
    /// or null if nothing is set for that key.
    /// </summary>
    ItemUnion? Make(string key);

    public CraftingRecipe MakeCraftingRecipe(CraftingRecipeDTO craftingRecipe)
    {
        var ingredients = craftingRecipe.Ingredients
            .GroupBy(stack => stack.ItemName)
            .Select(group => new ItemStack<Material>(
                MakeMaterial(group.Key),
                group.Sum(stack => stack.Quantity)
            ));

        var anItem = Make(craftingRecipe.ResultItemName) ?? throw new ArgumentException($"Bad item name: '{craftingRecipe.ResultItemName}'");
        return new CraftingRecipe(ingredients, () => anItem);
    }
    
    private Material MakeMaterial(string key)
    {
        var item = Make(key) ?? throw new ArgumentException($"No item registered with name '{key}'");
        
        Material? material = null;
        item.Match(
            m => material = m,
            _ => {}
        );
        if (material == null)
        {
            throw new ArgumentException($"Item with name '{key}' is not a material");
        }

        return material;
    }
}