using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives.Impl;

public class Cleave() : ActiveAbility("Cleave", 15, Duration.FromSeconds(1.5))
{
    public override IActiveAbility Copy() => new Cleave();

    public override string GetDescription()
    {
        var result = $"A melee attack which deals {GetDamage()} damage to nearby enemies.";
        return result;
    }

    protected override void OnUse(Direction inDirection, Attack attack)
    {
        SpawnProjectile(inDirection, attack, AttackRange.MELEE);
        SpawnProjectile(inDirection.TurnedClockwiseByDegrees(45), attack, AttackRange.MELEE);
        SpawnProjectile(inDirection.TurnedCounterClockwiseByDegrees(45), attack, AttackRange.MELEE);
    }

    protected override int GetDamage() => base.GetDamage()/2;
}