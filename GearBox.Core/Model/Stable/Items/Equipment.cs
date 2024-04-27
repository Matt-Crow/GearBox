using GearBox.Core.Model.Dynamic.Player;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public abstract class Equipment : IItem
{
    public Equipment(
        ItemType type, 
        string? description = null, 
        int? level = null,
        PlayerStatBoosts? statBoosts = null, 
        Guid? id = null
    )
    {
        Type = type;
        Description = description ?? "no description provided";
        Level = level ?? 0;
        StatBoosts = statBoosts ?? new PlayerStatBoosts();
        Id = id ?? Guid.NewGuid();
    }
    
    public Guid? Id { get; init; }
    
    public ItemType Type { get; init; }

    public string Description { get; init; }
    public int Level { get; init; }
    
    public abstract IEnumerable<string> Details { get; }
    
    /// <summary>
    /// The stat boosts this provides when equipped
    /// </summary>
    public PlayerStatBoosts StatBoosts { get; init; }

    /// <summary>
    /// Subclasses should override this if they can change at runtime
    /// </summary>
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>();

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
    public abstract Equipment ToOwned();
}