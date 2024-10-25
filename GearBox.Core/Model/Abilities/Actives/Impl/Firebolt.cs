using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives.Impl;

public class Firebolt() : ActiveAbility("Fire Bolt", 40, Duration.FromSeconds(3))
{
    public override IActiveAbility Copy() => new Firebolt();

    public override string GetDescription(Character character)
    {
        var result = $"A long-range attack which deals {GetDamageWhenUsedBy(character)} damage.";
        return result;
    }

    protected override void OnUse(Character user, Direction inDirection, Attack attack)
    {
        SpawnProjectile(user, inDirection, attack, AttackRange.LONG);
    }

    protected override int GetDamageWhenUsedBy(Character user) => base.GetDamageWhenUsedBy(user) * 5;
}
