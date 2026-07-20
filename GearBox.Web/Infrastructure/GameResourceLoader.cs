using System.Text.Json;
using GearBox.Core.Model.ResourcePacks;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the game-resources folder
/// </summary>
public class GameResourceLoader
{
    public async Task<ResourcePack> LoadDefaultResourcePack()
    {
        var resourceFilePath = Path.Combine("game-resources", "default.json");
        var resourcePack = await TryDeserialize<ResourcePack>(resourceFilePath);
        return resourcePack;
    }

    public async Task<AreaResource> LoadAreaByName(string name)
    {
        if (!name.All(IsAllowedFileNameCharacter))
        {
            throw new ArgumentException($"Invalid area name: {name}");
        }

        var filePath = Path.Combine("game-resources", "maps", name + ".json");
        var json = await TryDeserialize<AreaResource>(filePath);
        return json;
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