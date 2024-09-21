using GearBox.Core.Config;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies;

public class EnemyFactory : IEnemyFactory
{
    private readonly GearBoxConfig _config;
    private readonly IEnemyRepository _allEnemies;
    private readonly List<string> _names = [];
    private int _childCount = 0;

    public EnemyFactory(GearBoxConfig config, IEnemyRepository allEnemies)
    {
        _config = config;
        _allEnemies = allEnemies;
    }

    public static EnemyFactory MakeDefault() => new EnemyFactory(new GearBoxConfig(), new EnemyRepository(new ItemFactory()));

    public IEnemyFactory Add(string name)
    {
        var _ = _allEnemies.GetEnemyByName(name, 1) ?? throw new ArgumentException($"Bad enemy name: {name}", nameof(name));
        _names.Add(name);
        return this;
    }

    public EnemyCharacter? MakeRandom(int level)
    {
        if (!_names.Any())
        {
            return null;
        }
        var name = _names[Random.Shared.Next(_names.Count)];
        var result = _allEnemies.GetEnemyByName(name, level) ?? throw new Exception($"Bad enemy name: {name}");

        if (_config.DisableAI)
        {
            result.AiBehavior = new NullAiBehavior();
            result.StopMoving();
        }

        return result;
    }

    public GameTimer MakeSpawnTimer(IArea area)
    {
        int periodInFrames = Duration.FromSeconds(_config.EnemySpawning.PeriodInSeconds).InFrames;
        var result = new GameTimer(() => SpawnWave(area), periodInFrames);
        return result;
    }

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
        
        enemy.Termination.Terminated += (sender, e) => _childCount--;
        
        _childCount++;
    }
}