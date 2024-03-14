using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Stable.Items;

public class Weapon : Equipment
{
    private readonly AttackRange _attackRange;
    private readonly WeaponStats _stats;
    
    public Weapon(ItemType type, string? description = null, Guid? id = null, AttackRange? attackRange = null, WeaponStats? stats = null) : base(type, description, stats?.PlayerStatBoosts, id)
    {
        _attackRange = attackRange ?? AttackRange.MELEE;
        _stats = stats ?? new WeaponStats(0, StatBoosts);
    }

    public override EquipmentSlot GetSlot(PlayerCharacter player)
    {
        return player.Weapon;
    }
}