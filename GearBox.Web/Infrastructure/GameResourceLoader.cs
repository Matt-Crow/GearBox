using System.Text.Json;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Items;
using GearBox.Web.Model.Json;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public class GameResourceLoader
{
    private readonly IActiveAbilityFactory _actives;

    public GameResourceLoader(IActiveAbilityFactory actives)
    {
        _actives = actives;
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
        return json.ToMap();
    }

    public async Task<List<ItemUnion>> LoadAllItems()
    {
        var allItems = new List<ItemUnion>();
        var itemsFolder = Path.Combine("game-resources", "items");

        // materials are probably better off stored in CSV files though
        var materialsFolder = Path.Combine(itemsFolder, "materials");
        var materials = await LoadItems<MaterialJson>(materialsFolder);
        allItems.AddRange(materials);

        var weaponsFolder = Path.Combine(itemsFolder, "equipment", "weapons");
        var weapons = await LoadItems<WeaponJson>(weaponsFolder);
        allItems.AddRange(weapons);

        var armorsFolder = Path.Combine(itemsFolder, "equipment", "armors");
        var armors = await LoadItems<ArmorJson>(armorsFolder);
        allItems.AddRange(armors);

        return allItems;
    }

    private async Task<List<ItemUnion>> LoadItems<T>(string folderPath)
    where T : IItemJson
    {
        var items = new List<ItemUnion>();
        foreach (var filePath in Directory.EnumerateFiles(folderPath, "*.json"))
        {
            var item = await LoadItem<T>(filePath);
            items.Add(item);
        }
        return items;
    }

    private async Task<ItemUnion> LoadItem<T>(string filePath)
    where T : IItemJson
    {
        var text = await File.ReadAllTextAsync(filePath);
        var json = JsonSerializer.Deserialize<T>(text) ?? throw new Exception($"Failed to deserialize {filePath}");
        var item = json.ToItem(_actives);
        return item;
    }

    /// <summary>
    /// Validate path characters to prevent path injection
    /// </summary>
    private static bool IsAllowedFileNameCharacter(char ch) => char.IsLetterOrDigit(ch) || ch == '-';
}