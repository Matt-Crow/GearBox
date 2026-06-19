using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// An Equipment is a type of item which a player can wield
/// </summary>
public class Equipment : IItem
{
    private readonly Dictionary<PlayerStatType, int> _statWeights;


    public Equipment(
        string name, 
        EquipmentSlotType slotType,
        Grade? grade = null,
        Dictionary<PlayerStatType, int>? statWeights = null,
        IEnumerable<IActiveAbility>? actives = null,
        IEnumerable<IPassiveAbility>? passives = null,
        int? level = null
    )
    {
        Name = name;
        SlotType = slotType;
        Grade = grade ?? Grade.COMMON;
        Level = level ?? 1;
        BuyValue = new Gold(Grade.BuyValueBase * (Level + Character.MAX_LEVEL / 2));
        _statWeights = statWeights ?? [];
        StatBoosts = new PlayerStatBoosts(_statWeights, GetStatPoints(Level, Grade));
        Actives = actives ?? [];
        Passives = passives ?? [];
    }
    
    public Guid? Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }

    /// <summary>
    /// The type of slot this can be equipped in
    /// </summary>
    public EquipmentSlotType SlotType { get; init; }

    public Grade Grade { get; init; }

    /// <summary>
    /// The minimum level players must have to equip this
    /// </summary>
    public int Level { get; init; }
    
    public Gold BuyValue { get; init; }
    
    /// <summary>
    /// Details to display in the GUI
    /// </summary>
    public IEnumerable<string> Details => StatBoosts.Details;
    
    /// <summary>
    /// The stat boosts this provides when equipped
    /// </summary>
    public PlayerStatBoosts StatBoosts { get; init; }

    /// <summary>
    /// The active abilities this provides when equipped
    /// </summary>
    public IEnumerable<IActiveAbility> Actives { get; init; }

    /// <summary>
    /// The passive abilities this provides when equipped
    /// </summary>
    public IEnumerable<IPassiveAbility> Passives { get; init; }

    /// <summary>
    /// Returns this if it is immutable, or a clone otherwise
    /// </summary>
    public Equipment ToOwned(int? level=null)
    {
        var newLevel = level ?? Level;
        var newActives = Actives.Select(a => a.Copy()).ToList();
        var newPassives = Passives.Select(p => p.Copy()).ToList();
        var result = new Equipment(Name, SlotType, Grade, _statWeights, newActives, newPassives, newLevel);
        return result;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Equipment;
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
            SlotType.Name,
            Details,
            quantity,
            Actives
                .Select(a => new ActiveAbilityJson(a))
                .ToList(),
            Passives
                .Select(p => new PassiveAbilityJson(p))
                .ToList()
        );
        return result;
    }

    public static int GetStatPoints(int level, Grade grade)
    {
        var maxPoints = 2000;
        var minPoints = 200;
        var maxLevel = Character.MAX_LEVEL;
        var percentage = ((double)level) / maxLevel;
        var result = (int)(minPoints + percentage*(maxPoints - minPoints));
        return (int)(result * grade.PointMultiplier / EquipmentSlotType.ALL.Count());
    }
}