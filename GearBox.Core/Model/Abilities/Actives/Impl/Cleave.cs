using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives.Impl;

public class Cleave() : ActiveAbility("Cleave", 15, Duration.FromSeconds(1.5))
{
    public override IActiveAbility Copy() => new Cleave();

    public override string GetDescription(Character character)
    {
        var result = $"A melee attack which deals {GetDamageWhenUsedBy(character)} damage to nearby enemies.";
        return result;
    }

    protected override void OnUse(Character user, Direction inDirection, Attack attack)
    {
        SpawnProjectile(user, inDirection, attack, AttackRange.MELEE);
        SpawnProjectile(user, inDirection.TurnedClockwiseByDegrees(45), attack, AttackRange.MELEE);
        SpawnProjectile(user, inDirection.TurnedCounterClockwiseByDegrees(45), attack, AttackRange.MELEE);
    }

    protected override int GetDamageWhenUsedBy(Character user) => base.GetDamageWhenUsedBy(user)/2;
}