
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class WanderAiBehavior : IAiBehavior
{
    private readonly EnemyCharacter _target;
    private readonly IRandomNumberGenerator _rng;
    private int _framesLeftToWander;

    public WanderAiBehavior(EnemyCharacter target, IRandomNumberGenerator rng)
    {
        _target = target;
        _rng = rng;
        ChooseNewDirection();
    }

    private void ChooseNewDirection()
    {
        var seconds = _rng.Next(3, 20+1);
        _framesLeftToWander = Duration.FromSeconds(seconds).InFrames;

        var angle = _rng.Next(360);
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

        var nearestEnemy = _target.CurrentArea?.GetNearestPlayerTo(_target);
        if (nearestEnemy != null && IsCloseEnoughToSee(nearestEnemy))
        {
            _target.AiBehavior = new PursueAiBehavior(_target, nearestEnemy, _rng);
        }
    }

    private bool IsCloseEnoughToSee(PlayerCharacter player)
    {
        var distance = _target.Coordinates.DistanceFrom(player.Coordinates);
        return distance.InTiles <= 5;
    }
}