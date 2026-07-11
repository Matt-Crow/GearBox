namespace GearBox.Core.Model.ResourcePacks;

/// <summary>
/// Holds all the resources in a resource pack
/// </summary>
public class ResourcePack
{
    public required List<MaterialResource> Materials { get; set; }
    public required List<PartResource> Parts { get; set; }
    public required List<CraftingRecipeResource> CraftingRecipes { get; set; }
    public required List<EnemyResource> Enemies { get; set; }
}