using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly int _level;
    private Map? _map;
    private readonly List<ItemShopBuilder> _shopBuilders = [];
    private readonly IItemFactory _itemFactory;
    private readonly LootTableBuilder _lootBuilder;
    private readonly IEnemyFactory _enemies;
    private readonly List<IExit> _exits = [];

    public AreaBuilder(string name, int level, IItemFactory itemFactory, IEnemyFactory enemies, IRandomNumberGenerator rng)
    {
        Name = name;
        _level = level;
        _itemFactory = itemFactory;
        _lootBuilder = new(itemFactory, rng);
        _enemies = enemies;
    }

    /// <summary>
    /// The name of the area this is building
    /// </summary>
    public string Name { get; init; }

    public AreaBuilder AddLoot(Action<LootTableBuilder> withLoot)
    {
        withLoot(_lootBuilder);
        return this;
    }

    public AreaBuilder AddEnemies(Action<IEnemyFactory> withEnemies)
    {
        withEnemies(_enemies);
        return this;
    }

    public AreaBuilder WithMap(Map map)
    {
        _map = map;
        return this;
    }

    public AreaBuilder AddShop(string name, Coordinates coordinates, Color color, Func<ItemShopBuilder, ItemShopBuilder> withShopBuilder)
    {
        var sb = new ItemShopBuilder(name, coordinates, color, _itemFactory);
        withShopBuilder(sb);
        _shopBuilders.Add(sb);
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
            _shopBuilders.Select(sb => sb.Build()).ToList(),
            _lootBuilder.Build(_level),
            _enemies,
            _exits
        );
        result.AddTimer(new GameTimer(() => result.SpawnLootChest(), Duration.FromSeconds(10).InFrames));

        return result;
    }
}