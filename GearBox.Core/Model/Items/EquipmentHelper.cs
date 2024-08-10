using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Items;

public static class EquipmentHelper
{
    public static int GetStatPoints(int level, Grade grade)
    {
        var maxPoints = 1000;
        var minPoints = 100;
        var maxLevel = Character.MAX_LEVEL;
        var percentage = ((double)level) / maxLevel;
        var result = (int)(minPoints + percentage*(maxPoints - minPoints));
        return (int)(result * grade.PointMultiplier);
    }
}