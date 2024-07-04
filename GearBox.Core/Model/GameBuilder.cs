using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model;

public class GameBuilder : IGameBuilder
{
    private readonly HashSet<ItemType> _itemTypes = [];
    private readonly HashSet<CraftingRecipe> _craftingRecipes = [];
    private readonly List<AreaBuilder> _areas = [];

    public IGameBuilder WithItemType(ItemType itemType)
    {
        _itemTypes.Add(itemType);
        return this;
    }

    public IGameBuilder WithCraftingRecipe(CraftingRecipe recipe)
    {
        _craftingRecipes.Add(recipe);
        return this;
    }

    public IGameBuilder WithArea(Func<AreaBuilder, AreaBuilder> defineArea)
    {
        _areas.Add(defineArea(new AreaBuilder(this)));
        return this;
    }

    public IGame Build()
    {
        var result = new Game(
            ItemTypeRepository.Of(_itemTypes),
            CraftingRecipeRepository.Of(_craftingRecipes)
        );
        foreach (var area in _areas)
        {
            result.AddArea(area.Build(result));
        }
        return result;
    }
}