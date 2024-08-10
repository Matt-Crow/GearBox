using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public static class WeaponHelper
{
    public static int GetStatPoints(int level, Grade grade, AttackRange range)
    {
        return (int)(range.WeaponStatMultiplier * EquipmentHelper.GetStatPoints(level, grade));
    }
}