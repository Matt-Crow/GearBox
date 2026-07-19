using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly AreaResource _area;
    private Map? _map;
    private readonly Factory<ItemUnion> _itemFactory;
    private readonly EnemyFactory _enemies;
    private readonly IRandomNumberGenerator _rng;

    public AreaBuilder(AreaResource area, Factory<ItemUnion> itemFactory, EnemyFactory enemies, IRandomNumberGenerator rng)
    {
        _area = area;
        _itemFactory = itemFactory;
        _enemies = enemies;
        _rng = rng;
    }

    /// <summary>
    /// The name of the area this is building
    /// </summary>
    public string Name => _area.Name;


    public AreaBuilder WithMap(Map map)
    {
        _map = map;
        return this;
    }

    public Area Build(IGame game)
    {
        if (_map == null)
        {
            throw new Exception("map is required");
        }

        var lootOptions = _area.LootOptions
            .Select(resource => resource.ToLootOption(_itemFactory).ToLevel(_area.Level))
            .ToList();

        _enemies.CanSpawn(_area.EnemyNames);
        
        var result = new Area(
            _area.Name,
            _area.Level,
            game,
            _map,
            _area.Shops
                .Select(resource => resource.ToItemShop(_itemFactory))
                .ToList(),
            new LootTable(lootOptions, _rng),
            _enemies,
            _area.Exits
                .Select(resource => resource.ToExit())
                .ToList()
        );
        result.AddTimer(new GameTimer(() => result.SpawnLootChest(), Duration.FromSeconds(10).InFrames));

        return result;
    }
}