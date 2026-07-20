using System.Text.Json;
using GearBox.Core.Model.ResourcePacks;

namespace GearBox.Web.Infrastructure;

/// <summary>
/// Loads resources from the resource-packs folder
/// </summary>
public class GameResourceLoader
{
    public static async Task<ResourcePack> LoadDefaultResourcePack()
    {
        var resourceFilePath = Path.Combine("resource-packs", "default.json");
        var text = await File.ReadAllTextAsync(resourceFilePath);
        var resourcePack = JsonSerializer.Deserialize<ResourcePack>(text) ?? throw new Exception($"Failed to deserialize {resourceFilePath}");
        return resourcePack;
    }
}