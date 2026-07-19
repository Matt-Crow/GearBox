namespace GearBox.Core.Model.ResourcePacks;

public class AreaResource
{
    public required string Name { get; init; }
    public required int Level { get; init; }
    //public required MapResource Map { get; init; }
    public List<LootOptionResource> LootOptions { get; init; } = [];
    public List<ItemShopResource> Shops { get; init; } = [];
    public List<string> EnemyNames { get; init; } = [];
    public List<ExitResource> Exits { get; init; } = [];
    
}