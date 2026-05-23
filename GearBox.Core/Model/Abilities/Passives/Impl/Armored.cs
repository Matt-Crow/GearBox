using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives.Impl;

public class Armored : PassiveAbility
{
    private readonly double _reduction;

    private Armored(string name, double reduction) : base(name)
    {
        _reduction = reduction;
    }

    public static Armored Lightly() => new Armored("Lightly Armored", 0.2);
    public static Armored Moderately() => new Armored("Moderately Armored", 0.35);
    public static Armored Heavily() => new Armored("Heavily Armored", 0.5);

    public override IPassiveAbility Copy()
    {
        return new Armored(Name, _reduction);
    }

    public override string GetDescription()
    {
        return $"Reduces damage taken by {_reduction:P0}.";
    }

    private DamagedEvent HandleDamagedEvent(DamagedEvent e)
    {
        var multiplier = 1 - _reduction; // ex if reduce by 75%, multiplier is 25%
        var newDamage = e.Damage * multiplier;
        return e.WithDamage((int)newDamage);
    }

    protected override void RegisterTo(Character user)
    {
        user.EventDamaged.AddModifier(HandleDamagedEvent);
    }

    protected override void UnregisterFrom(Character user)
    {
        user.EventDamaged.RemoveModifier(HandleDamagedEvent);
    }
}