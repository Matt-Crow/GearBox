using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Ai;

public class PursueAiBehavior : IAiBehavior
{
    private readonly Character _controlling;
    private readonly Character _pursuing;

    public PursueAiBehavior(Character controlling, Character pursuing)
    {
        _controlling = controlling;
        _pursuing = pursuing;
    }

    public void Update()
    {
        // turn to them
        var newDirection = Direction.FromAToB(_controlling.Coordinates, _pursuing.Coordinates);
        _controlling.StartMovingIn(newDirection);

        // check if close enough to attack
        if (_controlling.BasicAttack.CanReach(_pursuing))
        {
            _controlling.AiBehavior = new AttackAiBehavior(_controlling, _pursuing);
        }
    }
}