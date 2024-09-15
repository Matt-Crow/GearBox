namespace GearBox.Core.Model.Items;

public interface IEquipmentStats
{
    IEnumerable<string> Details { get; }

    int GetStatPoints(int level, Grade grade);
}