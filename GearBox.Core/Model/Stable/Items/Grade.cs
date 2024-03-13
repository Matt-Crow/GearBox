using System.Collections.Immutable;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Each item type has a Grade which determines how common or powerful it is
/// </summary>
public readonly struct Grade
{
    public static readonly Grade COMMON = new (1, "Common", 100);
    public static readonly Grade UNCOMMON = new (2, "Uncommon", 50);
    public static readonly Grade RARE = new (3, "Rare", 15);
    public static readonly Grade EPIC = new (4, "Epic", 3);
    public static readonly Grade LEGENDARY = new (5, "Legendary", 1);
    public static readonly ImmutableList<Grade> ALL = ImmutableList.Create(
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        LEGENDARY
    );

    private Grade(int order, string name, int weight)
    {
        Order = order;
        Name = name;
        Weight = weight;
    }

    public int Order { get; init; }
    public string Name { get; init; }
    public int Weight { get; init; }
}