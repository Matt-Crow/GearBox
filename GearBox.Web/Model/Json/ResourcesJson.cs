namespace GearBox.Web.Model.Json;

/// <summary>
/// Holds all the resources in a resource pack
/// </summary>
public class ResourcesJson
{
    public required List<MaterialJson> Materials { get; set; }
    public required List<PartJson> Parts { get; set; }
    public required List<CraftingRecipeJson> CraftingRecipes { get; set; }
    public required List<EnemyJson> Enemies { get; set; }
}