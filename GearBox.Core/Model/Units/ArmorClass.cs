namespace GearBox.Core.Model.Units;

public readonly struct ArmorClass
{
    private readonly string _asString;
    private readonly double _damageReduction;


    public static readonly ArmorClass NONE = new("No armor", 0.0, 1.0);
    public static readonly ArmorClass LIGHT = new("Light armor", 0.2, 1.0);
    public static readonly ArmorClass MEDIUM = new("Medium armor", 0.35, 0.85);
    public static readonly ArmorClass HEAVY = new("Heavy armor", 0.5, 0.7);


    private ArmorClass(string asString, double damageReduction, double armorStatMultiplier)
    {
        _asString = asString;
        _damageReduction = damageReduction;
        ArmorStatMultiplier = armorStatMultiplier;
    }

    public double ArmorStatMultiplier { get; init; }
    
    public int ReduceDamage(int damage)
    {
        var multiplier = 1.0 - _damageReduction;
        var result = multiplier * damage;
        return (int)result;
    }

    public override string ToString() => _asString;
}