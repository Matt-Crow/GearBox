using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Passives.Impl;

/// <summary>
/// Note that there is a bug where,
/// if a player has multiple Ranged passives,
/// then unregisters one of them,
/// then their range will rever back to melee.
/// </summary>
public class Ranged : PassiveAbility
{
    private readonly AttackRange _range;


    private Ranged(string name, AttackRange range) : base(name)
    {
        _range = range;
    }


    public static Ranged Moderately() => new Ranged("Moderately Ranged", AttackRange.MEDIUM);
    public static Ranged Long() => new Ranged("Long Ranged", AttackRange.LONG);


    public override IPassiveAbility Copy() => new Ranged(Name, _range);

    public override string GetDescription()
    {
        var loweredRange = _range.ToString().ToLower();
        var description = $"The user's basic attacks can strike from {loweredRange}.";
        return description;
    }

    protected override void RegisterTo(Character user)
    {
        user.BasicAttack.Range = _range;
    }

    protected override void UnregisterFrom(Character user)
    {
        user.BasicAttack.Range = AttackRange.MELEE;
    }
}