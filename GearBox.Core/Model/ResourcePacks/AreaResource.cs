using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class AreaResource
{
    public required string Name { get; init; }
    public required int Level { get; init; }
    public required MapResource Map { get; init; }
    public List<LootOptionResource> LootOptions { get; init; } = [];
    public List<ItemShopResource> Shops { get; init; } = [];
    public List<string> EnemyNames { get; init; } = [];
    public List<ExitResource> Exits { get; init; } = [];


    public Area ToArea(IGame game, Factory<ItemUnion> itemFactory, EnemyFactory enemies, IRandomNumberGenerator rng)
    {
        var lootOptions = LootOptions
            .Select(resource => resource.ToLootOption(itemFactory).ToLevel(Level))
            .ToList();

        enemies.CanSpawn(EnemyNames);
        
        var result = new Area(
            Name,
            Level,
            game,
            Map.ToMap(rng),
            Shops
                .Select(resource => resource.ToItemShop(itemFactory))
                .ToList(),
            new LootTable(lootOptions, rng),
            enemies,
            Exits
                .Select(resource => resource.ToExit())
                .ToList()
        );
        result.AddTimer(new GameTimer(() => result.SpawnLootChest(), Duration.FromSeconds(10).InFrames));

        return result;
    }
}