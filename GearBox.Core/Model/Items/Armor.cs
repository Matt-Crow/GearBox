using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

public class Armor : Equipment
{
    public Armor(
        ItemType type,
        int? level = null,
        Guid? id = null,
        ArmorClass? armorClass = null,
        PlayerStatBoosts? boosts = null
    ) : base(type, level, boosts, id)
    {
        ArmorClass = armorClass ?? ArmorClass.NONE;
    }

    public ArmorClass ArmorClass { get; init; }

    public override IEnumerable<string> Details => ListExtensions.Of($"Class: {ArmorClass}")
        .Concat(StatBoosts.Details);

    public override Armor ToOwned()
    {
        return new Armor(Type, Level, Guid.NewGuid(), ArmorClass, StatBoosts);
    }
}