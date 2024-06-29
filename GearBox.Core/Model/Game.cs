using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Json.AreaInit;

namespace GearBox.Core.Model;

public class Game : IGame
{
    private readonly List<IArea> _areas = [];
    private readonly IItemTypeRepository _itemTypes;
    private readonly CraftingRecipeRepository _craftingRecipes;

    public Game(
        IItemTypeRepository? itemTypes = null,
        CraftingRecipeRepository? craftingRecipes = null)
    {
        _itemTypes = itemTypes ?? ItemTypeRepository.Empty();
        _craftingRecipes = craftingRecipes ?? CraftingRecipeRepository.Empty();
    }

    public List<ItemTypeJson> GetItemTypeJsons() => _itemTypes.ToJson();
    public List<CraftingRecipeJson> GetCraftingRecipeJsons() => _craftingRecipes.ToJson();
    public CraftingRecipe? GetCraftingRecipeById(Guid id) => _craftingRecipes.GetById(id);

    // cannot create game & area at the same time do to circular dependency
    public void AddArea(IArea area)
    {
        _areas.Add(area);
    }

    public IArea GetDefaultArea()
    {
        // todo some other way of signifying default area
        return _areas.FirstOrDefault() ?? throw new Exception("Game has no area");
    }
}