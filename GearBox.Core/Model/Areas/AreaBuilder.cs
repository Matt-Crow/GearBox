using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly IGameBuilder _gameBuilder;
    private Map? _map;
    private readonly LootTableBuilder _lootBuilder;
    private readonly List<Func<Character>> _enemies = [];
    private readonly List<IExit> _exits = [];

    public AreaBuilder(string name, IGameBuilder gameBuilder, IItemFactory itemFactory)
    {
        Name = name;
        _gameBuilder = gameBuilder;
        _lootBuilder = new(itemFactory);
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

    public AreaBuilder DefineEnemy(Func<Character> definition)
    {
        _enemies.Add(definition);
        return this;
    }

    public AreaBuilder WithMap(Map map)
    {
        _map = map;
        return this;
    }

    public AreaBuilder WithExit(IExit exit)
    {
        _exits.Add(exit);
        return this;
    }

    public AreaBuilder AddDefaultEnemies()
    {
        var result = this
            .DefineEnemy(() => new Character("Snake", 1, Color.LIGHT_GREEN))
            .DefineEnemy(() => new Character("Scorpion", 1, Color.BLACK))
            .DefineEnemy(() => new Character("Jackal", 2, Color.TAN));
        return result;
    }

    public Area Build(IGame game)
    {
        if (_map == null)
        {
            throw new Exception("map is required");
        }
        var result = new Area(
            Name,
            game,
            _map,
            _lootBuilder.Build(),
            _enemies,
            _exits
        );
        result.AddTimer(new GameTimer(result.SpawnLootChest, Duration.FromSeconds(10).InFrames));
        var spawner = new EnemySpawner(result, new EnemySpawnerOptions()
        {
            WaveSize = 3,
            MaxChildren = 10
        });
        result.AddTimer(new GameTimer(spawner.SpawnWave, Duration.FromSeconds(10).InFrames));

        return result;
    }
}