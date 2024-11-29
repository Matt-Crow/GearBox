using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives.Impl;

public class BasicAttack() : ActiveAbility("Basic Attack", 0, Duration.FromSeconds(0.5))
{
    public AttackRange Range { get; set; } = AttackRange.MELEE;

    public bool CanReach(Character user, Character target)
    {
        var distance = user.Coordinates.DistanceFrom(target.Coordinates);
        return distance.InPixels <= Range.Range.InPixels;
    }

    public override string GetDescription()
    {
        var result = $"A melee attack which deals {GetDamage()} damage.";
        return result;
    }

    protected override void OnUse(Direction inDirection, Attack attack)
    {
        SpawnProjectile(inDirection, attack, Range);
    }

    public override BasicAttack Copy() => new BasicAttack();
}