using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives;

public class BasicAttack() : ActiveAbility(0, Duration.FromSeconds(0.5))
{
    public AttackRange Range { get; set; } = AttackRange.MELEE;

    public bool CanReach(Character user, Character target)
    {
        var distance = user.Coordinates.DistanceFrom(target.Coordinates);
        return distance.InPixels <= Range.Range.InPixels;
    }

    protected override void OnUse(Character user, Direction inDirection)
    {
        SpawnProjectile(user, inDirection, Range, 1.0);
    }
}