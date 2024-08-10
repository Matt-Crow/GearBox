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

    public override Weapon DoBuild(int level, Dictionary<PlayerStatType, int> statWeights)
    {
        var stats = new PlayerStatBoosts(statWeights, WeaponHelper.GetStatPoints(level, ItemType.Grade, _attackRange));
        var result = new Weapon(
            ItemType, 
            level,
            null, // id is null
            _attackRange,
            stats
        );
        return result;
    }
}