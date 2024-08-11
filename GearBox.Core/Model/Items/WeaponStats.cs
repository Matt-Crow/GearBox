using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public class WeaponStats : IEquipmentStats
{
    public WeaponStats(AttackRange? attackRange = null)
    {
        AttackRange = attackRange ?? AttackRange.MELEE;
    }

    public AttackRange AttackRange { get; init; }

    public IEnumerable<string> Details => [$"Range: {AttackRange}"];

    public int GetStatPoints(int level, Grade grade) => (int)(Equipment<WeaponStats>.GetStatPoints(level, grade) * AttackRange.WeaponStatMultiplier);
}