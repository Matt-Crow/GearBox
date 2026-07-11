using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Infrastructure;

namespace GearBox.Core.Model.ResourcePacks;

public class EnemyResource
{
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required List<LootOptionResource> Loot { get; set; }


    public EnemyCharacterTemplate ToEnemyCharacterTemplate(IItemFactory items)
    {
        var color = GearBox.Core.Model.Color.FromName(Color) ?? throw new Exception($"Invalid color: '{Color}'");
        var loot = Loot
            .Select(json => json.ToLootOption(items))
            .ToList();
        return new EnemyCharacterTemplate(Name, color, loot);
    }
}