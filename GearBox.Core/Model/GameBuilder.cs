using GearBox.Core.Config;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Utils;

namespace GearBox.Core.Model;

public class GameBuilder : IGameBuilder
{
    private readonly GearBoxConfig _config;
    private readonly HashSet<CraftingRecipe> _craftingRecipes = [];
    private readonly List<AreaBuilder> _areas = []; // must be ordered so the first area added is the default area

    public GameBuilder(GearBoxConfig config)
    {
        _config = config;
        Enemies = new EnemyRepository(Items);
    }

    public IActiveAbilityFactory Actives { get; init; } = new ActiveAbilityFactory();
    public IItemFactory Items { get; init; } = new ItemFactory();
    public IEnemyRepository Enemies { get; init; }

    public IGameBuilder AddCraftingRecipe(Func<CraftingRecipeBuilder, CraftingRecipe> recipe)
    {
        var builder = new CraftingRecipeBuilder(Items);
        _craftingRecipes.Add(recipe(builder));
        return this;
    }

    public IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea)
    {
        if (_areas.Any(b => b.Name == name))
        {
            throw new ArgumentException("Name must be unique within each game", nameof(name));
        }

        // inject this instead if needed
        var rng = new RandomNumberGenerator();

        _areas.Add(defineArea(new AreaBuilder(name, level, Items, new EnemyFactory(_config, Enemies, rng))));
        return this;
    }

    public IGame Build()
    {
        var result = new Game(CraftingRecipeRepository.Of(_craftingRecipes));
        foreach (var area in _areas)
        {
            result.AddArea(area.Build(result));
        }
        return result;
    }
}