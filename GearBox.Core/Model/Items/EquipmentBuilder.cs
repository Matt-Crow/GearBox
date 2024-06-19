using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items;

public abstract class EquipmentBuilder<T>
where T : Equipment
{
    private Dictionary<PlayerStatType, int>? _statWeights;
    
    public EquipmentBuilder(ItemType type)
    {
        ItemType = type;
    }

    public ItemType ItemType { get; init; }

    public EquipmentBuilder<T> WithStatWeights(Dictionary<PlayerStatType, int> statWeights)
    {
        _statWeights = statWeights;
        return this;
    }

    /// <summary>
    /// Modifies total stat points provided by the equipment
    /// </summary>
    protected abstract int ModifyPoints(int points);

    /// <summary>
    /// Builds the equipment at the given level
    /// </summary>
    public abstract T DoBuild(int level, PlayerStatBoosts statBoosts);

    public T Build(int level)
    {
        var totalPoints = ModifyPoints(PointsForLevel(level));
        var statBoosts = new PlayerStatBoosts(_statWeights, totalPoints);
        return DoBuild(level, statBoosts);
    }

    private int PointsForLevel(int level)
    {
        var maxPoints = 1000;
        var minPoints = 100;
        var maxLevel = Character.MAX_LEVEL;
        var percentage = ((double)level) / maxLevel;
        var result = (int)(minPoints + percentage*(maxPoints - minPoints));
        return (int)(result * ItemType.Grade.PointMultiplier);
    }
}