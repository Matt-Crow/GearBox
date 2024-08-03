using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.Items.Crafting;

public class CraftingRecipeBuilder
{
    private readonly Dictionary<Material, int> _ingredients = [];
    private readonly IItemFactory _itemFactory;

    public CraftingRecipeBuilder(IItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
    }

    public CraftingRecipeBuilder And(string materialName, int quantity=1)
    {
        var material = _itemFactory.Make(materialName)?.Material ?? throw new ArgumentException($"Bad material name: '{materialName}'");
        if (!_ingredients.ContainsKey(material))
        {
            _ingredients[material] = 0;
        }
        _ingredients[material] += quantity;
        return this;
    }

    public CraftingRecipe Makes(string itemName)
    {
        var ingredients = _ingredients.Select(kv => new ItemStack<Material>(kv.Key, kv.Value));
        var anItem = _itemFactory.Make(itemName) ?? throw new ArgumentException($"Bad item name: '{itemName}'");
        return new CraftingRecipe(ingredients, () => anItem);
    }
}