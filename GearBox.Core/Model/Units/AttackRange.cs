namespace GearBox.Core.Model.Units;

public readonly struct AttackRange
{
    public static readonly AttackRange MELEE = new(Distance.FromTiles(1), 0.0);
    public static readonly AttackRange MEDIUM = new(Distance.FromTiles(5), 0.1);
    public static readonly AttackRange LONG = new(Distance.FromTiles(10), 0.2);

    private AttackRange(Distance range, double weaponStatPenalty)
    {
        Range = range;
        WeaponStatPenalty = weaponStatPenalty;
    }

    /// <summary>
    /// How far this attack can travel
    /// </summary>
    public Distance Range { get; init; }

    /// <summary>
    /// Weapons with this AttackRange have their stats reduced by this amount
    /// </summary>
    public double WeaponStatPenalty { get; init; }
}