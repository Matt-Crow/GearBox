using GearBox.Core.Model.Units;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable.Items;

public class Weapon : Equipment
{
    private readonly AttackRange _attackRange;
    
    public Weapon(
        ItemType type, 
        string? description = null, 
        int? level = null,
        Guid? id = null, 
        AttackRange? attackRange = null, 
        PlayerStatBoosts? boosts = null
    ) : base(type, description, level, boosts, id)
    {
        _attackRange = attackRange ?? AttackRange.MELEE;
    }

    public override IEnumerable<string> Details => ListExtensions.Of($"Range: {_attackRange}")
        .Concat(StatBoosts.Details);

    public override EquipmentSlot GetSlot(PlayerCharacter player) => player.Weapon;
}