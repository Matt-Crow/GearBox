using GearBox.Core.Config;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.GameObjects.Enemies;

/// <summary>
/// Handles enemies players may encounter in an area
/// </summary>
public class EnemyFactory
{
    private readonly GearBoxConfig _config;
    private readonly Factory<EnemyCharacterTemplate> _allEnemies;
    private readonly IRandomNumberGenerator _rng;
    private readonly List<string> _names = [];
    private int _childCount = 0;

    public EnemyFactory(GearBoxConfig config, Factory<EnemyCharacterTemplate> allEnemies, IRandomNumberGenerator rng)
    {
        _config = config;
        _allEnemies = allEnemies;
        _rng = rng;
    }

    public static EnemyFactory MakeDefault() => new EnemyFactory(new GearBoxConfig(), Factory<EnemyCharacterTemplate>.Of(e => e, []), new RandomNumberGenerator());


    public void CanSpawn(List<string> enemyNames)
    {
        _names.AddRange(enemyNames);
    }

    public EnemyCharacter? MakeRandom(int level)
    {
        if (!_names.Any())
        {
            return null;
        }
        var name = _rng.ChooseRandom(_names);
        var result = GetEnemyByName(name, level) ?? throw new Exception($"Bad enemy name: {name}");

        if (!_config.DisableAI)
        {
            result.AiBehavior = new WanderAiBehavior(result, _rng);
        }

        result.EventKilled.AddListener(e => HandleKilled(result, e));

        return result;
    }

    private EnemyCharacter? GetEnemyByName(string name, int level)
    {
        var template = _allEnemies.Make(name);

        var lootOptions = template.LootOptions
            .Select(opt => opt.ToLevel(level))
            .ToList();

        var enemy = new EnemyCharacter(
            template.Name, 
            level, 
            template.Color, 
            new LootTable(lootOptions, _rng)
        );
        return enemy;
    }

    private void HandleKilled(EnemyCharacter enemy, KilledEvent e)
    {
        if (e.AttackEvent.AttackUsed.UsedBy is not PlayerCharacter player)
        {
            return;
        }

        player.GainXp((int)(enemy.Level * _config.EnemyXpDropMultiplier));

        if (!_rng.CheckChance(_config.EnemyLootDropChance))
        {
            return;
        }

        var loot = enemy.Loot.GetRandomLoot();
        player.Inventory.Add(loot);
    }

    public GameTimer MakeSpawnTimer(IArea area) => new GameTimer(() => SpawnWave(area), _config.EnemySpawning.PeriodInFrames);

    private void SpawnWave(IArea area)
    {
        for (var i = 0; i < _config.EnemySpawning.WaveSize; i++)
        {
            Spawn(area);
        }
    }

    private void Spawn(IArea area)
    {
        if (_childCount >= _config.EnemySpawning.MaxEnemies)
        {
            return;
        }

        var enemy = area.SpawnEnemy();
        if (enemy == null)
        {
            return;
        }

        enemy.Termination.EventTerminated.AddListener(_ => _childCount--);
        
        _childCount++;
    }
}