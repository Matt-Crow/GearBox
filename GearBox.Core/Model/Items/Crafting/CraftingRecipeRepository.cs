using System.Collections.Frozen;

namespace GearBox.Core.Model.Items.Crafting;

public class CraftingRecipeRepository
{
    private readonly FrozenDictionary<Guid, CraftingRecipeDTO> _recipes;

    private CraftingRecipeRepository(IEnumerable<CraftingRecipeDTO> recipes)
    {
        _recipes = recipes.ToFrozenDictionary(recipe => recipe.Id, recipe => recipe);
    }

    public static CraftingRecipeRepository Of(IEnumerable<CraftingRecipeDTO> recipes)
    {
        return new CraftingRecipeRepository(recipes);
    }

    public static CraftingRecipeRepository Empty()
    {
        return Of([]);
    }


    public IEnumerable<CraftingRecipeDTO> All => _recipes.Values;


    public CraftingRecipeDTO? GetById(Guid id)
    {
        _recipes.TryGetValue(id, out CraftingRecipeDTO? result);
        return result;
    }
}