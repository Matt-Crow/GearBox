using System.Collections.Immutable;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Each item type has a Grade which determines how common or powerful it is
/// </summary>
public readonly struct Grade
{
    public static readonly Grade COMMON = new (1, "Common", 100, 0.5);
    public static readonly Grade UNCOMMON = new (2, "Uncommon", 50, 0.6);
    public static readonly Grade RARE = new (3, "Rare", 15, 0.7);
    public static readonly Grade EPIC = new (4, "Epic", 3, 0.8);
    public static readonly Grade LEGENDARY = new (5, "Legendary", 1, 1.0);
    public static readonly ImmutableList<Grade> ALL = [COMMON, UNCOMMON, RARE, EPIC, LEGENDARY];

    private Grade(int order, string name, int weight, double pointMultiplier)
    {
        Order = order;
        Name = name;
        Weight = weight;
        PointMultiplier = pointMultiplier;
    }

    public int Order { get; init; }
    public string Name { get; init; }
    public int Weight { get; init; }

    /// <summary>
    /// Equipment of this grade has its total stat points multiplied by this
    /// </summary>
    public double PointMultiplier { get; init; }
}