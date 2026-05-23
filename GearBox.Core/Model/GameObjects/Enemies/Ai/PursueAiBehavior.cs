using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class PursueAiBehavior : IAiBehavior
{
    private readonly EnemyCharacter _controlling;
    private readonly PlayerCharacter _pursuing;
    private readonly IRandomNumberGenerator _rng;

    public PursueAiBehavior(EnemyCharacter controlling, PlayerCharacter pursuing, IRandomNumberGenerator rng)
    {
        _controlling = controlling;
        _pursuing = pursuing;
        _rng = rng;

        // wander around once the target gets away
        pursuing.EventAreaChanged.AddListener(
            _ => _controlling.AiBehavior = new WanderAiBehavior(_controlling, _rng),
            1 // only run once
        );
    }

    public void Update()
    {
        // turn to them
        var newDirection = Direction.FromAToB(_controlling.Coordinates, _pursuing.Coordinates);
        _controlling.StartMovingIn(newDirection);

        // check if close enough to attack
        if (_controlling.BasicAttack.CanReach(_controlling, _pursuing))
        {
            _controlling.AiBehavior = new AttackAiBehavior(_controlling, _pursuing, _rng);
        }
    }
}