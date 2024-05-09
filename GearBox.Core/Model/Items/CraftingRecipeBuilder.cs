namespace GearBox.Core.Model.Items;

public class CraftingRecipeBuilder
{
    private readonly Dictionary<Material, int> _ingredients = [];

    public CraftingRecipeBuilder And(Material ingredient, int quantity=1)
    {
        return And(new ItemStack<Material>(ingredient, quantity));
    }

    public CraftingRecipeBuilder And(ItemStack<Material> ingredient)
    {
        if (!_ingredients.ContainsKey(ingredient.Item))
        {
            _ingredients[ingredient.Item] = 0;
        }
        _ingredients[ingredient.Item] += ingredient.Quantity;
        return this;
    }

    public CraftingRecipe Makes(Func<ItemUnion> anItem)
    {
        var result = new CraftingRecipe(_ingredients.Select(kv => new ItemStack<Material>(kv.Key, kv.Value)), anItem);
        return result;
    }
}