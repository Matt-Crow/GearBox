using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Json;

/// <summary>
/// Combines together data from ItemStack, IItem, and ItemType
/// </summary>
public readonly struct ItemJson : IChange, IJson
{
    public ItemJson(
        Guid? id,
        string name,
        string gradeName,
        int gradeOrder,
        string description,
        int level,
        string slotType,
        IEnumerable<string> details,
        int quantity,
        List<ActiveAbilityJson> actives,
        List<PassiveAbilityJson> passives
    )
    {
        Id = id;
        Name = name;
        GradeName = gradeName;
        GradeOrder = gradeOrder;
        Description = description;
        Level = level;
        SlotType = slotType;
        Details = details;
        Quantity = quantity;
        Actives = actives;
        Passives = passives;
    }

    public Guid? Id { get; init; }
    public string Name { get; init; }
    public string GradeName { get; init; }
    public int GradeOrder { get; init; }
    public string Description { get; init; }
    public int Level { get; init; }
    public string SlotType { get; init; }
    public IEnumerable<string> Details { get; init; }

    /// <summary>
    /// The number of items in this stack
    /// </summary>
    public int Quantity { get; init; }

    public List<ActiveAbilityJson> Actives { get; init; }
    public List<PassiveAbilityJson> Passives { get; init; }

    public IEnumerable<object?> DynamicValues => [Id, Name, Description, Level, SlotType, .. Details, Quantity, .. Actives.SelectMany(a => a.DynamicValues), .. Passives.SelectMany(p => p.DynamicValues)];
}