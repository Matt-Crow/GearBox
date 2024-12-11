using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class AttackAiBehavior : IAiBehavior
{
    private readonly EnemyCharacter _controlling;
    private readonly PlayerCharacter _attacking;

    public AttackAiBehavior(EnemyCharacter controlling, PlayerCharacter attacking)
    {
        _controlling = controlling;
        _attacking = attacking;
        attacking.AreaChanged += HandleAreaChangedEvent;
    }

    private void HandleAreaChangedEvent(object? sender, AreaChangedEventArgs e)
    {
        _controlling.AiBehavior = new WanderAiBehavior(_controlling);
        e.WhoChangedAreas.AreaChanged -= HandleAreaChangedEvent;
    }

    public void Update()
    {
        if (_attacking.Termination.IsTerminated)
        {
            _controlling.AiBehavior = new WanderAiBehavior(_controlling);
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
            _controlling.AiBehavior = new PursueAiBehavior(_controlling, _attacking);
        }
    }
}
