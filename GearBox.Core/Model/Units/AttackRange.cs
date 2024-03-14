namespace GearBox.Core.Model.Units;

public readonly struct AttackRange
{
    private readonly string _asString;
    public static readonly AttackRange MELEE = new("Melee range", Distance.FromTiles(1), 0.0);
    public static readonly AttackRange MEDIUM = new("Medium range", Distance.FromTiles(5), 0.1);
    public static readonly AttackRange LONG = new("Long range", Distance.FromTiles(10), 0.2);

    private AttackRange(string asString, Distance range, double weaponStatPenalty)
    {
        _asString = asString;
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

    public override string ToString() => _asString;
}