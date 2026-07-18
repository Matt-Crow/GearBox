using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class CraftingRecipeResource
{
    public required string ResultItemName { get; set; }
    public required List<ItemStackResource<Material>> Ingredients { get; set; }

    public CraftingRecipe ToCraftingRecipe(Factory<ItemUnion> items)
    {
        var result = new CraftingRecipe(
            Ingredients
                .Select(json => json.ToItemStackOfMaterial(items))
                .ToList(),
            items.Make(ResultItemName)
        );
        return result;
    }
}