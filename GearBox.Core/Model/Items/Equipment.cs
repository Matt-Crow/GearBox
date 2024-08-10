using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public abstract class Equipment : IItem
{
    public Equipment(
        ItemType type, 
        int? level = null,
        PlayerStatBoosts? statBoosts = null, 
        Guid? id = null
    )
    {
        Type = type;
        Level = level ?? 0;
        StatBoosts = statBoosts ?? PlayerStatBoosts.Empty();
        Id = id ?? Guid.NewGuid();
    }
    
    public Guid? Id { get; init; }
    
    public ItemType Type { get; init; }

    public string Description => ""; // equipment doesn't need a description
    public int Level { get; init; }
    
    public abstract IEnumerable<string> Details { get; }
    
    /// <summary>
    /// The stat boosts this provides when equipped
    /// </summary>
    public PlayerStatBoosts StatBoosts { get; init; }

    public override bool Equals(object? obj)
    {
        var other = obj as Equipment;
        return other?.Id == Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Returns this if it is immutable, or a clone otherwise
    /// </summary>
    public abstract Equipment ToOwned(int? level=null);

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