using GearBox.Core.Model.Areas;
namespace GearBox.Core.Model.GameObjects;

public class EnemySpawner 
{
    private readonly IArea _area;
    private readonly EnemySpawnerOptions _options;
    private int _childCount = 0;

    public EnemySpawner(IArea area, EnemySpawnerOptions? options=null)
    {
        _area = area;
        _options = options ?? new();
    }

    public void SpawnWave()
    {
        for (var i = 0; i < _options.WaveSize; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_childCount >= _options.MaxChildren)
        {
            return;
        }

        var enemy = _area.SpawnEnemy();
        if (enemy.Termination != null)
        {
            enemy.Termination.Terminated += ChildTerminated;
        }
        _childCount++;
    }

    private void ChildTerminated(object? sender, TerminateEventArgs e)
    {
        _childCount--;
    }
}
