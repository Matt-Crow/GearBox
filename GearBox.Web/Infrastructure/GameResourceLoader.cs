using System.Text.Json;
using GearBox.Core.Model.Static;
using GearBox.Web.Model.Json;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public static class GameResourceLoader
{
    public static async Task<Map> LoadMapByName(string name)
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

    private static bool IsAllowedFileNameCharacter(char ch)
    {
        if (char.IsLetterOrDigit(ch))
        {
            return true;
        }
        return ch == '-';
    }
}