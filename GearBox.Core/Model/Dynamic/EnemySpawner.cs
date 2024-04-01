using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Dynamic;

public class EnemySpawner : IDynamicGameObject
{
    private static readonly Duration COOLDOWN = Duration.FromSeconds(10);
    private readonly World _world;
    private readonly Func<Coordinates, IDynamicGameObject> _factory;
    private readonly EnemySpawnerOptions _options;
    private int _childCount = 0;
    private int _framesUntilNextUse = 0;

    public EnemySpawner(World world, Func<Coordinates, IDynamicGameObject> factory, EnemySpawnerOptions? options=null)
    {
        _world = world;
        _factory = factory;
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

        var tile = _world.Map.GetRandomOpenTile();
        if (tile == null)
        {
            return;
        }

        var enemy = _factory.Invoke(tile.Value.CenteredOnTile());
        if (enemy.Termination != null)
        {
            enemy.Termination.Terminated += ChildTerminated;
        }
        _world.DynamicContent.AddDynamicObject(enemy);
        _childCount++;
    }

    private void ChildTerminated(object? sender, TerminateEventArgs e)
    {
        _childCount--;
    }

}
