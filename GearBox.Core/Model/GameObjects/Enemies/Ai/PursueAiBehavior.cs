using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects.Enemies.Ai;

public class PursueAiBehavior : IAiBehavior
{
    private readonly Character _controlling;
    private readonly Character _pursuing;

    public PursueAiBehavior(Character controlling, Character pursuing)
    {
        _controlling = controlling;
        _pursuing = pursuing;

        /* 
            While I don't like this cast, it's required so I can uphold a few rules:
            1. players can change areas, but characters cannot (cannot push AreaChanged to superclass)
            2. characters controlled by AI can attack other characters controlled by AI (cannot pull attacking to subclass)
            3. AttackAiBehavior is instantiated by PursueAiBehavior, which also targets characters (cannot overload ctor)
        */
        if (pursuing is PlayerCharacter player)
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