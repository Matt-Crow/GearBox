using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

public class Weapon : Equipment
{
    public Weapon(
        ItemType type, 
        string? description = null, 
        int? level = null,
        Guid? id = null, 
        AttackRange? attackRange = null, 
        PlayerStatBoosts? boosts = null
    ) : base(type, description, level, boosts, id)
    {
        AttackRange = attackRange ?? AttackRange.MELEE;
    }

    public AttackRange AttackRange { get; init; }
    public override IEnumerable<string> Details => ListExtensions.Of($"Range: {AttackRange}")
        .Concat(StatBoosts.Details);

    public override Weapon ToOwned()
    {
        return new Weapon(Type, Description, Level, Guid.NewGuid(), AttackRange, StatBoosts);
    }
}