using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Web.Model.Json;

public class CraftingRecipeJson
{
    public required string ResultItemName { get; set; }
    public required List<ItemStackJson> Ingredients { get; set; }

    public CraftingRecipeDTO ToCraftingRecipeDTO()
    {
        var result = new CraftingRecipeDTO(
            Ingredients
                .Select(json => json.ToItemStackDTO())
                .ToList(),
            ResultItemName
        );
        return result;
    }
}