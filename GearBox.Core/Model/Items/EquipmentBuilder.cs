using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items;

// todo not needed?
public class EquipmentBuilder<T>
where T : IEquipmentStats
{
    private Dictionary<PlayerStatType, int>? _statWeights;
    
    public EquipmentBuilder(ItemType type, T inner)
    {
        ItemType = type;
        Inner = inner;
    }

    public ItemType ItemType { get; init; }
    protected T Inner { get; set;}

    public EquipmentBuilder<T> WithStatWeights(Dictionary<PlayerStatType, int> statWeights)
    {
        _statWeights = statWeights;
        return this;
    }

    public virtual Equipment<T> Build(int level)
    {
        var stats = new PlayerStatBoosts(_statWeights, Inner.GetStatPoints(level, ItemType.Grade));
        var result = new Equipment<T>(
            ItemType, 
            Inner,
            level,
            stats,
            null // id is null
        );
        return result;
    }
}