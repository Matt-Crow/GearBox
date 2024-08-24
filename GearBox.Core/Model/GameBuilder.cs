using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model;

public class GameBuilder : IGameBuilder
{
    private readonly HashSet<CraftingRecipe> _craftingRecipes = [];
    private readonly List<AreaBuilder> _areas = []; // must be ordered so the first area added is the default area
    private readonly IItemFactory _itemFactory = new ItemFactory();
    private readonly IEnemyRepository _enemies = new EnemyRepository();

    public IGameBuilder DefineItems(Func<IItemFactory, IItemFactory> defineItems)
    {
        defineItems(_itemFactory);
        return this;
    }

    public IGameBuilder DefineEnemies(Func<IEnemyRepository, IEnemyRepository> defineEnemies)
    {
        defineEnemies(_enemies);
        return this;
    }

    
    public IGameBuilder AddCraftingRecipe(Func<CraftingRecipeBuilder, CraftingRecipe> recipe)
    {
        var builder = new CraftingRecipeBuilder(_itemFactory);
        _craftingRecipes.Add(recipe(builder));
        return this;
    }

    public IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea)
    {
        if (_areas.Any(b => b.Name == name))
        {
            throw new ArgumentException("Name must be unique within each game", nameof(name));
        }
        _areas.Add(defineArea(new AreaBuilder(name, level, _itemFactory, _enemies)));
        return this;
    }

    public IGame Build()
    {
        var result = new Game(
            ItemTypeRepository.Of(_itemFactory.AvailableTypes),
            CraftingRecipeRepository.Of(_craftingRecipes)
        );
        foreach (var area in _areas)
        {
            result.AddArea(area.Build(result));
        }
        return result;
    }
}