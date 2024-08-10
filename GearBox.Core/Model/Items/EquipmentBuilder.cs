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
    /// Builds the equipment at the given level
    /// </summary>
    public abstract T DoBuild(int level, Dictionary<PlayerStatType, int> statWeights);

    public T Build(int level) => DoBuild(level, _statWeights ?? []);
}