
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class WanderAiBehavior : IAiBehavior
{
    private readonly Character _target;
    private int _framesLeftToWander;

    public WanderAiBehavior(Character target)
    {
        _target = target;
        ChooseNewDirection();
    }

    private void ChooseNewDirection()
    {
        var seconds = Random.Shared.Next(3, 20+1);
        _framesLeftToWander = Duration.FromSeconds(seconds).InFrames;

        var angle = Random.Shared.Next(360);
        var direction = Direction.FromBearingDegrees(angle);
        _target.StartMovingIn(direction);
    }

    public void Update()
    {
        _framesLeftToWander--;
        if (_framesLeftToWander == 0)
        {
            ChooseNewDirection();
        }

        var nearestEnemy = _target.CurrentArea?.GetNearestEnemy(_target);
        if (nearestEnemy != null && IsCloseEnoughToSee(nearestEnemy))
        {
            _target.AiBehavior = new PursueAiBehavior(_target, nearestEnemy);
        }
    }

    private bool IsCloseEnoughToSee(Character enemy)
    {
        var distance = _target.Coordinates.DistanceFrom(enemy.Coordinates);
        return distance.InTiles <= 5;
    }
}