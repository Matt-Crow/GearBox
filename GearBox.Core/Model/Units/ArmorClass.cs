namespace GearBox.Core.Model.Units;

public readonly struct ArmorClass
{
    private readonly string _asString;


    public static readonly ArmorClass NONE = new("No armor", 0.0, 1.0);
    public static readonly ArmorClass LIGHT = new("Light armor", 0.2, 1.0);
    public static readonly ArmorClass MEDIUM = new("Medium armor", 0.35, 0.85);
    public static readonly ArmorClass HEAVY = new("Heavy armor", 0.5, 0.7);
    public static readonly IEnumerable<ArmorClass> ALL = [NONE, LIGHT, MEDIUM, HEAVY];

    private ArmorClass(string asString, double damageReduction, double armorStatMultiplier)
    {
        _asString = asString;
        DamageReduction = damageReduction;
        ArmorStatMultiplier = armorStatMultiplier;
    }

    public static ArmorClass? GetArmorClassByName(string name) => ALL.FirstOrDefault(x => x._asString == name || x._asString == name + " armor");

    public double ArmorStatMultiplier { get; init; }
    public double DamageReduction { get; init; }
    
    public int ReduceDamage(int damage)
    {
        var multiplier = 1.0 - DamageReduction;
        var result = multiplier * damage;
        return (int)result;
    }

    public override string ToString() => _asString;
}