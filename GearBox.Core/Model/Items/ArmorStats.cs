using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public class ArmorStats : IEquipmentStats
{
    public ArmorStats(ArmorClass? armorClass = null)
    {
        ArmorClass = armorClass ?? ArmorClass.NONE;
    }

    public ArmorClass ArmorClass { get; init; }
    public IEnumerable<string> Details => [$"Class: {ArmorClass}"];

    public int GetStatPoints(int level, Grade grade) => (int)(Equipment<WeaponStats>.GetStatPoints(level, grade) * ArmorClass.ArmorStatMultiplier);
}