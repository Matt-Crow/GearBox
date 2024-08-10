using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public static class ArmorHelper
{
    public static int GetStatPoints(int level, Grade grade, ArmorClass ac)
    {
        return (int)(ac.ArmorStatMultiplier * EquipmentHelper.GetStatPoints(level, grade));
    }
}