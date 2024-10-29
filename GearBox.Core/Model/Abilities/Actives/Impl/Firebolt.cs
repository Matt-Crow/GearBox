using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives.Impl;

public class Firebolt() : ActiveAbility("Fire Bolt", 40, Duration.FromSeconds(3))
{
    public override IActiveAbility Copy() => new Firebolt();

    public override string GetDescription()
    {
        var result = $"A long-range attack which deals {GetDamage()} damage.";
        return result;
    }

    protected override void OnUse(Direction inDirection, Attack attack)
    {
        SpawnProjectile(inDirection, attack, AttackRange.LONG);
    }

    protected override int GetDamage() => base.GetDamage() * 5;
}
