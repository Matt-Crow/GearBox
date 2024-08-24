using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class PursueAiBehavior : IAiBehavior
{
    private readonly EnemyCharacter _controlling;
    private readonly PlayerCharacter _pursuing;

    public PursueAiBehavior(EnemyCharacter controlling, PlayerCharacter pursuing)
    {
        _controlling = controlling;
        _pursuing = pursuing;
        pursuing.AreaChanged += HandleAreaChangedEvent;
    }

    private void HandleAreaChangedEvent(object? sender, AreaChangedEventArgs e)
    {
        _controlling.AiBehavior = new WanderAiBehavior(_controlling);
        e.WhoChangedAreas.AreaChanged -= HandleAreaChangedEvent;
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