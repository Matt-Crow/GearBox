using System.Text.Json;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils;
using GearBox.Web.Model.Json;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public class GameResourceLoader
{
    private readonly IActiveAbilityFactory _actives;
    private readonly IPassiveAbilityFactory _passives;
    private readonly IRandomNumberGenerator _rng;

    public GameResourceLoader(IActiveAbilityFactory actives, IPassiveAbilityFactory passives, IRandomNumberGenerator rng)
    {
        _actives = actives;
        _passives = passives;
        _rng = rng;
    }

    public async Task<Map> LoadMapByName(string name)
    {
        if (!name.All(IsAllowedFileNameCharacter))
        {
            throw new ArgumentException($"Invalid map name: {name}");
        }

        var path = Path.Combine("game-resources", "maps", name + ".json");
        var text = await File.ReadAllTextAsync(path);
        var json = JsonSerializer.Deserialize<MapResourceJson>(text) ?? throw new ArgumentException($"Map not found: {name}");
        return json.ToMap(_rng);
    }

    public async Task<List<ItemUnion>> LoadAllItems()
    {
        var allItems = new List<ItemUnion>();
        var itemsFolder = Path.Combine("game-resources", "items");

        // materials are probably better off stored in CSV files though
        var materials = await LoadItems<MaterialJson>(Path.Combine(itemsFolder, "materials.json"));
        allItems.AddRange(materials);

        var parts = await LoadItems<PartJson>(Path.Combine(itemsFolder, "parts.json"));
        allItems.AddRange(parts);

        return allItems;
    }

    private async Task<List<ItemUnion>> LoadItems<T>(string filePath)
    where T : IItemJson
    {
        var text = await File.ReadAllTextAsync(filePath);
        var json = JsonSerializer.Deserialize<List<T>>(text) ?? throw new Exception($"Failed to deserialize {filePath}");
        var items = json
            .Select(itemJson => itemJson.ToItem(_actives, _passives))
            .ToList();
        return items;
    }

    /// <summary>
    /// Validate path characters to prevent path injection
    /// </summary>
    private static bool IsAllowedFileNameCharacter(char ch) => char.IsLetterOrDigit(ch) || ch == '-';
}