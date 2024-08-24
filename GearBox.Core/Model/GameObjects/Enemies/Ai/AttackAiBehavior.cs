using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class AttackAiBehavior : IAiBehavior
{
    private readonly Character _controlling;
    private readonly Character _attacking;

    public AttackAiBehavior(Character controlling, Character attacking)
    {
        _controlling = controlling;
        _attacking = attacking;

        /* 
            While I don't like this cast, it's required so I can uphold a few rules:
            1. players can change areas, but characters cannot (cannot push AreaChanged to superclass)
            2. characters controlled by AI can attack other characters controlled by AI (cannot pull attacking to subclass)
            3. AttackAiBehavior is instantiated by PursueAiBehavior, which also targets characters (cannot overload ctor)
        */
        if (attacking is PlayerCharacter player)
        {
            player.AreaChanged += HandleAreaChangedEvent;
        }
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
        else if (_controlling.BasicAttack.CanReach(_attacking))
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
