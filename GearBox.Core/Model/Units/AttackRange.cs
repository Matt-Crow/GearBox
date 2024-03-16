namespace GearBox.Core.Model.Units;

public readonly struct AttackRange
{
    private readonly string _asString;
    public static readonly AttackRange MELEE = new("Melee range", Distance.FromTiles(1), 1.0);
    public static readonly AttackRange MEDIUM = new("Medium range", Distance.FromTiles(5), 0.9);
    public static readonly AttackRange LONG = new("Long range", Distance.FromTiles(10), 0.8);

    private AttackRange(string asString, Distance range, double weaponStatPenalty)
    {
        _asString = asString;
        Range = range;
        WeaponStatMultiplier = weaponStatPenalty;
    }

    /// <summary>
    /// How far this attack can travel
    /// </summary>
    public Distance Range { get; init; }

    /// <summary>
    /// Weapons with this AttackRange have their stats multiplied by this amount
    /// </summary>
    public double WeaponStatMultiplier { get; init; }

    public override string ToString() => _asString;
}