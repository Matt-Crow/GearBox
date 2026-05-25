namespace GearBox.Core.Model.Units;

public class AttackRange
{
    private readonly string _asString;
    public static readonly AttackRange MELEE = new("Melee range", Distance.FromTiles(1), Color.GRAY);
    public static readonly AttackRange MEDIUM = new("Medium range", Distance.FromTiles(5), Color.RED);
    public static readonly AttackRange LONG = new("Long range", Distance.FromTiles(10), Color.BLACK);


    private AttackRange(string asString, Distance range, Color projectileColor)
    {
        _asString = asString;
        Range = range;
        ProjectileColor = projectileColor;
    }


    /// <summary>
    /// How far this attack can travel
    /// </summary>
    public Distance Range { get; init; }

    public Color ProjectileColor { get; init; }

    public override string ToString() => _asString;
}