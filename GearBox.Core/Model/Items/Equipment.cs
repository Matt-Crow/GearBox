using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public class Equipment<T> : IItem
where T : IEquipmentStats
{
    private readonly Dictionary<PlayerStatType, int> _statWeights;

    public Equipment(
        ItemType type, 
        T inner,
        int? level = null,
        Dictionary<PlayerStatType, int>? statWeights = null
    )
    {
        Type = type;
        Inner = inner;
        Level = level ?? 0;
        _statWeights = statWeights ?? [];
        StatBoosts = new PlayerStatBoosts(_statWeights, Inner.GetStatPoints(Level, Type.Grade));
    }
    
    public Guid? Id { get; init; } = Guid.NewGuid();
    public ItemType Type { get; init; }
    public string Description => ""; // equipment doesn't need a description
    public int Level { get; init; }
    public T Inner { get;}
    
    public IEnumerable<string> Details => Inner.Details.Concat(StatBoosts.Details);
    
    /// <summary>
    /// The stat boosts this provides when equipped
    /// </summary>
    public PlayerStatBoosts StatBoosts { get; init; }

    /// <summary>
    /// Returns this if it is immutable, or a clone otherwise
    /// </summary>
    public Equipment<T> ToOwned(int? level=null)
    {
        var newLevel = level ?? Level;
        var result = new Equipment<T>(Type, Inner, newLevel, _statWeights);
        return result;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Equipment<T>;
        return other?.Id == Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

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