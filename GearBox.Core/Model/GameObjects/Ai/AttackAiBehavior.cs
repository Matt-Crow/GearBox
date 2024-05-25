using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Ai;

public class AttackAiBehavior : IAiBehavior
{
    private readonly Character _controlling;
    private readonly Character _attacking;

    public AttackAiBehavior(Character controlling, Character attacking)
    {
        _controlling = controlling;
        _attacking = attacking;
    }

    public void Update()
    {
        if (_attacking.Termination.IsTerminated)
        {
            _controlling.AiBehavior = new WanderAiBehavior(_controlling);
        }
        else if (_controlling.BasicAttack.CanReach(_attacking))
        {
            // turn and attack
            _controlling.StopMoving();
            var world = _controlling.World ?? throw new Exception("world should not be null here");
            var angle = Direction.FromAToB(_controlling.Coordinates, _attacking.Coordinates);
            _controlling.UseBasicAttack(world, angle);
        }
        else
        {
            _controlling.AiBehavior = new PursueAiBehavior(_controlling, _attacking);
        }
    }
}
