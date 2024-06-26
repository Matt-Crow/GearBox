using System.Collections.Frozen;
using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model.Items.Crafting;

public class CraftingRecipeRepository
{
    private readonly FrozenDictionary<Guid, CraftingRecipe> _recipes;

    private CraftingRecipeRepository(IEnumerable<CraftingRecipe> recipes)
    {
        _recipes = recipes.ToFrozenDictionary(recipe => recipe.Id, recipe => recipe);
    }

    public static CraftingRecipeRepository Of(IEnumerable<CraftingRecipe> recipes)
    {
        return new CraftingRecipeRepository(recipes);
    }

    public static CraftingRecipeRepository Empty()
    {
        return Of([]);
    }

    public List<CraftingRecipeJson> ToJson()
    {
        var result = _recipes.Values
            .Select(recipe => recipe.ToJson())
            .ToList();
        return result;
    }

    public CraftingRecipe? GetById(Guid id)
    {
        _recipes.TryGetValue(id, out CraftingRecipe? result);
        return result;
    }
}