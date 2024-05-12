using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items.Crafting;

/// <summary>
/// Details how to convert materials into another item
/// </summary>
public class CraftingRecipe
{
    public CraftingRecipe(IEnumerable<ItemStack<Material>> ingredients, Func<ItemUnion> maker)
    {
        Ingredients = ingredients;
        Maker = maker;
    }

    public IEnumerable<ItemStack<Material>> Ingredients { get; init; }
    public Func<ItemUnion> Maker { get; init; }

    public CraftingRecipeJson ToJson()
    {
        var ingredients = Ingredients
            .Select(stack => stack.ToJson())
            .ToList();
        var makes = Maker.Invoke().ToJson();
        var result = new CraftingRecipeJson(ingredients, makes);
        return result;
    }
}