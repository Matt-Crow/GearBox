using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Dynamic;

public class EnemySpawner : IDynamicGameObject
{
    private static readonly Duration COOLDOWN = Duration.FromSeconds(10);
    private readonly World _world;
    private readonly EnemySpawnerOptions _options;
    private int _childCount = 0;
    private int _framesUntilNextUse = 0;

    public EnemySpawner(World world, EnemySpawnerOptions? options=null)
    {
        _world = world;
        _options = options ?? new();
    }


    public BodyBehavior? Body => null;
    public Serializer? Serializer => null;
    public TerminateBehavior? Termination => null;
    

    public void Update()
    {
        _framesUntilNextUse--;
        if (_framesUntilNextUse <= 0)
        {
            for (var i = 0; i < _options.WaveSize; i++)
            {
                Spawn();
            }
            _framesUntilNextUse = COOLDOWN.InFrames;
        }
    }

    private void Spawn()
    {
        if (_childCount >= _options.MaxChildren)
        {
            return;
        }

        var enemy = _world.SpawnEnemy();
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
