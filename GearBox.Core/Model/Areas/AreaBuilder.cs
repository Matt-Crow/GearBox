using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

public class AreaBuilder
{
    private readonly int _level;
    private Map? _map;
    private readonly LootTableBuilder _lootBuilder;
    private readonly List<Func<int, EnemyCharacter>> _enemies = [];
    private readonly List<IExit> _exits = [];

    public AreaBuilder(string name, int level, IItemFactory itemFactory)
    {
        Name = name;
        _level = level;
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

    public AreaBuilder DefineEnemy(Func<int, EnemyCharacter> definition)
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
            .DefineEnemy(lv => new EnemyCharacter("Snake", lv, Color.LIGHT_GREEN))
            .DefineEnemy(lv => new EnemyCharacter("Scorpion", lv, Color.BLACK))
            .DefineEnemy(lv => new EnemyCharacter("Jackal", lv, Color.TAN));
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
            _level,
            game,
            _map,
            _lootBuilder.Build(_level),
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