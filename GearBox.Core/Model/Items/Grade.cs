namespace GearBox.Core.Model.Items;

/// <summary>
/// Each item type has a Grade which determines how common or powerful it is
/// </summary>
public class Grade
{
    public static readonly Grade COMMON = new (1, "Common", 100, 1, 0.5);
    public static readonly Grade UNCOMMON = new (2, "Uncommon", 50, 3, 0.6);
    public static readonly Grade RARE = new (3, "Rare", 15, 8, 0.7);
    public static readonly Grade EPIC = new (4, "Epic", 3, 16, 0.8);
    public static readonly Grade LEGENDARY = new (5, "Legendary", 1, 50, 1.0);

    public static readonly IEnumerable<Grade> ALL = [COMMON, UNCOMMON, RARE, EPIC, LEGENDARY];

    private Grade(int order, string name, int weight, int buyValueBase, double pointMultiplier)
    {
        Order = order;
        Name = name;
        Weight = weight;
        BuyValueBase = buyValueBase;
        PointMultiplier = pointMultiplier;
    }

    public static Grade? GetGradeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);

    public int Order { get; init; }
    public string Name { get; init; }
    public int Weight { get; init; }

    /// <summary>
    /// Used when calculating the gold value of an item of this grade
    /// </summary>
    public int BuyValueBase { get; init; }

    /// <summary>
    /// Equipment of this grade has its total stat points multiplied by this
    /// </summary>
    public double PointMultiplier { get; init; }
}