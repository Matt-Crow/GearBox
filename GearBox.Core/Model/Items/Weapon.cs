using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

public class Weapon : Equipment
{
    public Weapon(
        ItemType type, 
        int? level = null,
        Guid? id = null, 
        AttackRange? attackRange = null, 
        PlayerStatBoosts? boosts = null
    ) : base(type, level, boosts, id)
    {
        AttackRange = attackRange ?? AttackRange.MELEE;
    }

    public AttackRange AttackRange { get; init; }
    public override IEnumerable<string> Details => ListExtensions.Of($"Range: {AttackRange}")
        .Concat(StatBoosts.Details);

    public override Weapon ToOwned(int? level=null)
    {
        var newLevel = level ?? Level;
        var newStats = StatBoosts.WithTotalPoints(WeaponHelper.GetStatPoints(newLevel, Type.Grade, AttackRange));
        return new Weapon(Type, newLevel, Guid.NewGuid(), AttackRange, newStats);
    }
}