using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Dynamic;

public class EnemySpawner : IDynamicGameObject
{
    private static readonly Duration COOLDOWN = Duration.FromSeconds(10);
    private readonly World _world;
    private readonly int _waveSize;
    private readonly int _maxChildren;
    private int _childCount = 0;
    private int _framesUntilNextUse = 0;


    public EnemySpawner(World world, int waveSize=1, int maxChildren=5)
    {
        _world = world;
        _waveSize = waveSize;
        _maxChildren = maxChildren;
    }


    public BodyBehavior? Body => null;
    public Serializer? Serializer => null;
    public TerminateBehavior? Termination => null;
    

    public void Update()
    {
        _framesUntilNextUse--;
        if (_framesUntilNextUse <= 0)
        {
            for (var i = 0; i < _waveSize; i++)
            {
                Spawn();
            }
            _framesUntilNextUse = COOLDOWN.InFrames;
        }
    }

    private void Spawn()
    {
        if (_childCount >= _maxChildren)
        {
            return;
        }

        var tile = _world.Map.GetRandomOpenTile();
        if (tile == null)
        {
            return;
        }

        var enemy = new Character
        {
            Coordinates = tile.Value.CenteredOnTile()
        };
        enemy.Termination.Terminated += ChildTerminated;
        _world.DynamicContent.AddDynamicObject(enemy);
        _childCount++;
    }

    private void ChildTerminated(object? sender, TerminateEventArgs e)
    {
        _childCount--;
    }

}
