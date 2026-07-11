using System.Text.Json;
using GearBox.Core.Model;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;
using GearBox.Web.Model.Json;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public class GameResourceLoader
{
    private readonly Factory<IActiveAbility> _actives;
    private readonly Factory<IPassiveAbility> _passives;
    private readonly IRandomNumberGenerator _rng;

    public GameResourceLoader(Factory<IActiveAbility> actives, Factory<IPassiveAbility> passives, IRandomNumberGenerator rng)
    {
        _actives = actives;
        _passives = passives;
        _rng = rng;
    }


    /// <summary>
    /// Loads all resources from the default resource pack into the given game builder.
    /// </summary>
    public async Task LoadResourcesInto(IGameBuilder gameBuilder)
    {
        var resourceFilePath = Path.Combine("game-resources", "default.json");
        var resourcesJson = await TryDeserialize<ResourcesJson>(resourceFilePath);

        // load items first, as crafting recipes and enemies depend on them
        foreach (var material in resourcesJson.Materials)
        {
            gameBuilder.Items.Add(material.ToItem(_actives, _passives));
        }
        foreach (var part in resourcesJson.Parts)
        {
            gameBuilder.Items.Add(part.ToItem(_actives, _passives));
        }

        foreach (var recipe in resourcesJson.CraftingRecipes)
        {
            gameBuilder.AddCraftingRecipe(recipe.ToCraftingRecipe(gameBuilder.Items));
        }

        foreach (var enemy in resourcesJson.Enemies)
        {
            gameBuilder.Enemies.Add(enemy.ToEnemyCharacterTemplate(gameBuilder.Items));
        }
    }

    public async Task<Map> LoadMapByName(string name)
    {
        if (!name.All(IsAllowedFileNameCharacter))
        {
            throw new ArgumentException($"Invalid map name: {name}");
        }

        var filePath = Path.Combine("game-resources", "maps", name + ".json");
        var json = await TryDeserialize<MapResourceJson>(filePath);
        return json.ToMap(_rng);
    }

    private static async Task<T> TryDeserialize<T>(string filePath)
    {
        var text = await File.ReadAllTextAsync(filePath);
        var json = JsonSerializer.Deserialize<T>(text) ?? throw new Exception($"Failed to deserialize {filePath}");
        return json;
    }

    /// <summary>
    /// Validate path characters to prevent path injection
    /// </summary>
    private static bool IsAllowedFileNameCharacter(char ch) => char.IsLetterOrDigit(ch) || ch == '-';
}