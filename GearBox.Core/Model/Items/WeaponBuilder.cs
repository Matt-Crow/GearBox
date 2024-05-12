using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public class WeaponBuilder
{
    private string? _description;
    private AttackRange _attackRange = AttackRange.MELEE;
    private readonly WeaponStatWeights _statWeights = new();

    public WeaponBuilder(ItemType type)
    {
        ItemType = type;
    }

    public ItemType ItemType { get; init; }

    public WeaponBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public WeaponBuilder WithRange(AttackRange range)
    {
        _attackRange = range;
        return this;
    }

    public WeaponBuilder WithStatWeights(Action<WeaponStatWeights> action)
    {
        action(_statWeights);
        return this;
    }

    /// <summary>
    /// Builds the weapon at the given level
    /// </summary>
    public Weapon Build(int level)
    {
        var totalPoints = (int)(
            PointsForLevel(level) 
            * ItemType.Grade.PointMultiplier 
            * _attackRange.WeaponStatMultiplier
        );

        var result = new Weapon(
            ItemType, 
            _description,
            level,
            null, // id is null
            _attackRange,
            _statWeights.Build(totalPoints)
        );
        return result;
    }

    private static int PointsForLevel(int level)
    {
        var maxPoints = 1000;
        var minPoints = 100;
        var maxLevel = Character.MAX_LEVEL;
        var percentage = ((double)level) / maxLevel;
        var result = (int)(minPoints + percentage*(maxPoints - minPoints));
        return result;
    }
}