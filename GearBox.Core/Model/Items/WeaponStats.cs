namespace GearBox.Core.Model.Items;

public class WeaponStats : IEquipmentStats
{
    public IEnumerable<string> Details => [];

    public int GetStatPoints(int level, Grade grade) => Equipment<WeaponStats>.GetStatPoints(level, grade);
}