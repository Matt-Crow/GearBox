namespace GearBox.Core.Model.ResourcePacks;

/// <summary>
/// Holds all the resources in a resource pack
/// </summary>
public class ResourcePack
{
    public List<MaterialResource> Materials { get; init; } = [];
    public List<PartResource> Parts { get; init; } = [];
    public List<CraftingRecipeResource> CraftingRecipes { get; init; } = [];
    public List<EnemyResource> Enemies { get; init; } = [];
    public List<AreaResource> Areas { get; init; } = [];
}