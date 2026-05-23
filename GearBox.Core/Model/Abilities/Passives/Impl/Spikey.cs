using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives.Impl;

public class Spikey : PassiveAbility
{
    public Spikey() : base("Spikey")
    {
    }

    public override IPassiveAbility Copy() => new Spikey();

    public override string GetDescription() => "Damages enemies who attack the user.";

    protected override void RegisterTo(Character user)
    {
        user.EventAttacked.AddListener(DamageWhoeverAttackedTheUser);
    }

    protected override void UnregisterFrom(Character user)
    {
        user.EventAttacked.RemoveListener(DamageWhoeverAttackedTheUser);
    }

    private void DamageWhoeverAttackedTheUser(AttackedEvent e)
    {
        e.AttackUsed.UsedBy.TakeDamage(e.AttackUsed.Damage / 10);
    }
}