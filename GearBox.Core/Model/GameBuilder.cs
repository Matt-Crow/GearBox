using GearBox.Core.Config;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model;

public class GameBuilder : IGameBuilder
{
    private readonly GearBoxConfig _config;
    private readonly IRandomNumberGenerator _rng;
    private readonly Factory<IActiveAbility> _actives;
    private readonly Factory<IPassiveAbility> _passives;
    private readonly List<CraftingRecipe> _craftingRecipes = [];
    private readonly IEnemyRepository _enemies;
    private readonly List<AreaBuilder> _areas = []; // must be ordered so the first area added is the default area


    public GameBuilder(GearBoxConfig config, IRandomNumberGenerator rng, GameResources resources)
    {
        _config = config;
        _rng = rng;
        _actives = Factory<IActiveAbility>.Of(a => a.Copy(), resources.Actives);
        _passives = Factory<IPassiveAbility>.Of(p => p.Copy(), resources.Passives);
        _enemies = new EnemyRepository(rng);
        foreach (var resourcePack in resources.ResourcePacks)
        {
            LoadResourcePack(resourcePack);
        }
    }


    public IItemFactory Items { get; init; } = new ItemFactory();


    private void LoadResourcePack(ResourcePack resourcePack)
    {
        // load items first, as crafting recipes and enemies depend on them
        foreach (var material in resourcePack.Materials)
        {
            Items.Add(material.ToItem(_actives, _passives));
        }
        foreach (var part in resourcePack.Parts)
        {
            Items.Add(part.ToItem(_actives, _passives));
        }

        foreach (var recipe in resourcePack.CraftingRecipes)
        {
            _craftingRecipes.Add(recipe.ToCraftingRecipe(Items));
        }

        foreach (var enemy in resourcePack.Enemies)
        {
            _enemies.Add(enemy.ToEnemyCharacterTemplate(Items));
        }
    }

    public IGameBuilder WithArea(string name, int level, Func<AreaBuilder, AreaBuilder> defineArea)
    {
        if (_areas.Any(b => b.Name == name))
        {
            throw new ArgumentException("Name must be unique within each game", nameof(name));
        }

        _areas.Add(defineArea(new AreaBuilder(name, level, Items, new EnemyFactory(_config, _enemies, _rng), _rng)));
        return this;
    }

    public IGame Build()
    {
        var result = new Game(Factory<CraftingRecipe>.Of(cr => cr, _craftingRecipes));
        foreach (var area in _areas)
        {
            result.AddArea(area.Build(result));
        }
        return result;
    }
}