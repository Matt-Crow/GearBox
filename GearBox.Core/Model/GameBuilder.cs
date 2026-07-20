using GearBox.Core.Config;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
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
    private readonly List<CraftingRecipe> _craftingRecipes;
    private readonly Factory<EnemyCharacterTemplate> _enemies;
    private readonly List<AreaResource> _areas = []; // must be ordered so the first area added is the default area


    public GameBuilder(GearBoxConfig config, IRandomNumberGenerator rng, GameResources resources)
    {
        _config = config;
        _rng = rng;
        _actives = Factory<IActiveAbility>.Of(a => a.Copy(), resources.Actives);
        _passives = Factory<IPassiveAbility>.Of(p => p.Copy(), resources.Passives);
        
        // load items first, as crafting recipes and enemies depend on them
        var allMaterials = resources.ResourcePacks
            .SelectMany(rp => rp.Materials)
            .Select(material => material.ToItem(_actives, _passives));
        var allParts = resources.ResourcePacks
            .SelectMany(rp => rp.Parts)
            .Select(part => part.ToItem(_actives, _passives));
        var allItems = new List<ItemUnion>()
            .Concat(allMaterials)
            .Concat(allParts)
            .ToList();
        Items = Factory<ItemUnion>.Of(item => item.ToOwned(), allItems);

        _craftingRecipes = resources.ResourcePacks
            .SelectMany(rp => rp.CraftingRecipes)
            .Select(recipe => recipe.ToCraftingRecipe(Items))
            .ToList();

        var enemyTemplates = resources.ResourcePacks
            .SelectMany(rp => rp.Enemies)
            .Select(er => er.ToEnemyCharacterTemplate(Items))
            .ToList();
        
        _enemies = Factory<EnemyCharacterTemplate>.Of(e => e, enemyTemplates);
    }


    public Factory<ItemUnion> Items { get; init; }


    public IGameBuilder WithArea(AreaResource area)
    {
        if (_areas.Any(b => b.Name == area.Name))
        {
            throw new ArgumentException("Name must be unique within each game", nameof(area.Name));
        }

        _areas.Add(area);
        return this;
    }

    public IGame Build()
    {
        var result = new Game(Factory<CraftingRecipe>.Of(cr => cr, _craftingRecipes));
        foreach (var area in _areas)
        {
            result.AddArea(area.ToArea(result, Items, new EnemyFactory(_config, _enemies, _rng), _rng));
        }
        return result;
    }
}