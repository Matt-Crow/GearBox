using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Web.Model.Json;

public class CraftingRecipeJson
{
    public required string ResultItemName { get; set; }
    public required List<ItemStackJson<Material>> Ingredients { get; set; }

    public CraftingRecipe ToCraftingRecipe(IItemFactory items)
    {
        var result = new CraftingRecipe(
            Ingredients
                .Select(json => json.ToItemStackOfMaterial(items))
                .ToList(),
            items.MakeOrThrow(ResultItemName)
        );
        return result;
    }
}