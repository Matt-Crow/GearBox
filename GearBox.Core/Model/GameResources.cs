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

/// <summary>
/// Raw this available to a game.
/// This is as opposed to non-raw this such as enemies moving around
/// and non-this such as random number generators.
/// </summary>
public class GameResources
{
    public List<IActiveAbility> Actives { get; set; } = [];
    public List<IPassiveAbility> Passives { get; set; } = [];
    public List<ResourcePack> ResourcePacks { get; set; } = [];


    public IGame ToGame(GearBoxConfig config, IRandomNumberGenerator rng)
    {
        var actives = Factory<IActiveAbility>.Of(a => a.Copy(), Actives);
        var passives = Factory<IPassiveAbility>.Of(p => p.Copy(), Passives);

        // load items first, as crafting recipes and enemies depend on them
        var allMaterials = ResourcePacks
            .SelectMany(rp => rp.Materials)
            .Select(material => material.ToItem(actives, passives));
        var allParts = ResourcePacks
            .SelectMany(rp => rp.Parts)
            .Select(part => part.ToItem(actives, passives));
        var allItems = new List<ItemUnion>()
            .Concat(allMaterials)
            .Concat(allParts)
            .ToList();
        var items = Factory<ItemUnion>.Of(item => item.ToOwned(), allItems);

        // load crafting recipes
        var craftingRecipes = ResourcePacks
            .SelectMany(rp => rp.CraftingRecipes)
            .Select(recipe => recipe.ToCraftingRecipe(items))
            .ToList();

        // load enemies
        var enemyTemplates = ResourcePacks
            .SelectMany(rp => rp.Enemies)
            .Select(er => er.ToEnemyCharacterTemplate(items))
            .ToList();
        var enemies = Factory<EnemyCharacterTemplate>.Of(e => e, enemyTemplates);


        // done loading game-wide this, so we can load areas
        var areaResources = ResourcePacks.SelectMany(rp => rp.Areas);

        // check for duplicate area names
        var firstDuplicatedAreaName = areaResources
            .GroupBy(ar => ar.Name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .FirstOrDefault();
        if (firstDuplicatedAreaName != null)
        {
            throw new Exception($"Duplicated area name: '{firstDuplicatedAreaName}'");
        }

        var result = new Game(items, Factory<CraftingRecipe>.Of(cr => cr, craftingRecipes));
        foreach (var area in areaResources)
        {
            result.AddArea(area.ToArea(result, items, new EnemyFactory(config, enemies, rng), rng));
        }
        return result;
    }
}