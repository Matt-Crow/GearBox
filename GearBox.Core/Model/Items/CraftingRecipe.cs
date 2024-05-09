namespace GearBox.Core.Model.Items;

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
}