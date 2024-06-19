using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public class WeaponBuilder(ItemType type) : EquipmentBuilder<Weapon>(type)
{
    private AttackRange _attackRange = AttackRange.MELEE;

    public WeaponBuilder WithRange(AttackRange range)
    {
        _attackRange = range;
        return this;
    }

    protected override int ModifyPoints(int points)
    {
        return (int)(points * _attackRange.WeaponStatMultiplier);
    }

    public override Weapon DoBuild(int level, PlayerStatBoosts statBoosts)
    {
        var result = new Weapon(
            ItemType, 
            level,
            null, // id is null
            _attackRange,
            statBoosts
        );
        return result;
    }
}