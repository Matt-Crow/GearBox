using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public class Equipment<T> : IItem
where T : IEquipmentStats
{
    private readonly Dictionary<PlayerStatType, int> _statWeights;

    public Equipment(
        string name, 
        T inner,
        Grade? grade = null,
        Dictionary<PlayerStatType, int>? statWeights = null,
        IEnumerable<IActiveAbility>? actives = null,
        int? level = null
    )
    {
        Name = name;
        Inner = inner;
        Grade = grade ?? Grade.COMMON;
        Level = level ?? 1;
        BuyValue = new Gold(Grade.BuyValueBase * (Level + Character.MAX_LEVEL / 2));
        _statWeights = statWeights ?? [];
        StatBoosts = new PlayerStatBoosts(_statWeights, Inner.GetStatPoints(Level, Grade));
        Actives = actives ?? [];
    }
    
    public Guid? Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }
    public T Inner { get;}
    public Grade Grade { get; init; }

    /// <summary>
    /// The minimum level players must have to equip this
    /// </summary>
    public int Level { get; init; }
    
    public Gold BuyValue { get; init; }
    
    /// <summary>
    /// Details to display in the GUI
    /// </summary>
    public IEnumerable<string> Details => Inner.Details.Concat(StatBoosts.Details);
    
    /// <summary>
    /// The stat boosts this provides when equipped
    /// </summary>
    public PlayerStatBoosts StatBoosts { get; init; }

    /// <summary>
    /// The active abilities this provides when equipped
    /// </summary>
    public IEnumerable<IActiveAbility> Actives { get; init; }

    /// <summary>
    /// Returns this if it is immutable, or a clone otherwise
    /// </summary>
    public Equipment<T> ToOwned(int? level=null)
    {
        var newLevel = level ?? Level;
        var newActives = Actives.Select(a => a.Copy()).ToList();
        var result = new Equipment<T>(Name, Inner, Grade, _statWeights, newActives, newLevel);
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

    public ItemJson ToJson(int quantity)
    {
        var result = new ItemJson(
            Id,
            Name,
            Grade.Name,
            Grade.Order,
            "", // no description
            Level,
            Details,
            quantity,
            Actives
                .Select(a => new ActiveAbilityJson(a))
                .ToList()
        );
        return result;
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