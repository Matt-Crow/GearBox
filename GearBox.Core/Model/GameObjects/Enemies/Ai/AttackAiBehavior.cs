using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class AttackAiBehavior : IAiBehavior
{
    private readonly EnemyCharacter _controlling;
    private readonly PlayerCharacter _attacking;
    private readonly IRandomNumberGenerator _rng;

    public AttackAiBehavior(EnemyCharacter controlling, PlayerCharacter attacking, IRandomNumberGenerator rng)
    {
        _controlling = controlling;
        _attacking = attacking;
        _rng = rng;
        attacking.AreaChanged += HandleAreaChangedEvent;
    }

    private void HandleAreaChangedEvent(object? sender, AreaChangedEventArgs e)
    {
        _controlling.AiBehavior = new WanderAiBehavior(_controlling, _rng);
        e.WhoChangedAreas.AreaChanged -= HandleAreaChangedEvent;
    }

    public void Update()
    {
        if (_attacking.Termination.IsTerminated)
        {
            _controlling.AiBehavior = new WanderAiBehavior(_controlling, _rng);
        }
        else if (_controlling.BasicAttack.CanReach(_controlling, _attacking))
        {
            // turn and attack
            _controlling.StopMoving();
            var angle = Direction.FromAToB(_controlling.Coordinates, _attacking.Coordinates);
            _controlling.UseBasicAttack(angle);
        }
        else
        {
            _controlling.AiBehavior = new PursueAiBehavior(_controlling, _attacking, _rng);
        }
    }
}
