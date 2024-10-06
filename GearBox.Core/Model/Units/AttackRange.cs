namespace GearBox.Core.Model.Units;

public class AttackRange
{
    private readonly string _asString;
    public static readonly AttackRange MELEE = new("Melee range", Distance.FromTiles(1), 1.0, Color.GRAY);
    public static readonly AttackRange MEDIUM = new("Medium range", Distance.FromTiles(5), 0.9, Color.RED);
    public static readonly AttackRange LONG = new("Long range", Distance.FromTiles(10), 0.8, Color.BLACK);

    public static readonly IEnumerable<AttackRange> ALL = [MELEE, MEDIUM, LONG];

    private AttackRange(string asString, Distance range, double weaponStatPenalty, Color projectileColor)
    {
        _asString = asString;
        Range = range;
        WeaponStatMultiplier = weaponStatPenalty;
        ProjectileColor = projectileColor;
    }

    public static AttackRange? GetAttackRangeByName(string name) => ALL.FirstOrDefault(x => x._asString == name || x._asString == name + " range");

    /// <summary>
    /// How far this attack can travel
    /// </summary>
    public Distance Range { get; init; }

    public Color ProjectileColor { get; init; }

    /// <summary>
    /// Weapons with this AttackRange have their stats multiplied by this amount
    /// </summary>
    public double WeaponStatMultiplier { get; init; }

    public override string ToString() => _asString;
}