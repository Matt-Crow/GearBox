using System.Text.Json;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Utils;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public class GameResourceLoader
{
    private readonly IRandomNumberGenerator _rng;

    public GameResourceLoader(IRandomNumberGenerator rng)
    {
        _rng = rng;
    }


    public async Task<ResourcePack> LoadDefaultResourcePack()
    {
        var resourceFilePath = Path.Combine("game-resources", "default.json");
        var resourcePack = await TryDeserialize<ResourcePack>(resourceFilePath);
        return resourcePack;
    }

    public async Task<Map> LoadMapByName(string name)
    {
        if (!name.All(IsAllowedFileNameCharacter))
        {
            throw new ArgumentException($"Invalid map name: {name}");
        }

        var filePath = Path.Combine("game-resources", "maps", name + ".json");
        var json = await TryDeserialize<MapResource>(filePath);
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