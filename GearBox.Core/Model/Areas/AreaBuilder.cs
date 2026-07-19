using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly int _level;
    private Map? _map;
    private readonly List<ItemShop> _shops = [];
    private readonly Factory<ItemUnion> _itemFactory;
    private readonly List<LootOption> _lootOptions = [];
    private readonly EnemyFactory _enemies;
    private readonly List<IExit> _exits = [];
    private readonly IRandomNumberGenerator _rng;

    public AreaBuilder(string name, int level, Factory<ItemUnion> itemFactory, EnemyFactory enemies, IRandomNumberGenerator rng)
    {
        Name = name;
        _level = level;
        _itemFactory = itemFactory;
        _enemies = enemies;
        _rng = rng;
    }

    /// <summary>
    /// The name of the area this is building
    /// </summary>
    public string Name { get; init; }


    public AreaBuilder AddLoot(List<LootOptionResource> lootOptions)
    {
        _lootOptions.AddRange(lootOptions.Select(lor => lor.ToLootOption(_itemFactory).ToLevel(_level)));
        return this;
    }

    public AreaBuilder AddEnemies(List<string> enemyNames)
    {
        _enemies.CanSpawn(enemyNames);
        return this;
    }

    public AreaBuilder WithMap(Map map)
    {
        _map = map;
        return this;
    }

    public AreaBuilder AddShop(ItemShopResource shop)
    {
        _shops.Add(shop.ToItemShop(_itemFactory));
        return this;
    }

    public AreaBuilder WithExit(IExit exit)
    {
        _exits.Add(exit);
        return this;
    }

    public Area Build(IGame game)
    {
        if (_map == null)
        {
            throw new Exception("map is required");
        }

        var result = new Area(
            Name,
            _level,
            game,
            _map,
            _shops,
            new LootTable(_lootOptions, _rng),
            _enemies,
            _exits
        );
        result.AddTimer(new GameTimer(() => result.SpawnLootChest(), Duration.FromSeconds(10).InFrames));

        return result;
    }
}