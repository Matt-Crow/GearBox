using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Stable.Items;

public class WeaponStatWeights
{
    private int _damagePerHit;
    private readonly Dictionary<PlayerStatType, int> _playerStatWeights = new();

    public WeaponStatWeights()
    {
        foreach (var playerStatType in PlayerStatType.ALL)
        {
            _playerStatWeights[playerStatType] = 0;
        }
    }

    public WeaponStatWeights WeighDamagePerHit(int weight)
    {
        _damagePerHit = weight;
        return this;
    }

    public WeaponStatWeights Weigh(PlayerStatType playerStatType, int weight)
    {
        _playerStatWeights[playerStatType] = weight;
        return this;
    }

    public WeaponStats Build(AttackRange attackRange)
    {
        // TODO scale with level & grade
        var totalPoints = (int)(1000 * (1-attackRange.WeaponStatPenalty));
        
        var totalWeights = _damagePerHit + _playerStatWeights.Values.Sum();
        if (totalWeights == 0)
        {
            totalWeights = 1; // prevent divide by 0
        }

        var weightedStatBoosts = _playerStatWeights.ToDictionary(
            x => x.Key,
            x => x.Value * totalPoints / totalWeights
        );

        var result = new WeaponStats(
            _damagePerHit * totalPoints / totalWeights,
            new PlayerStatBoosts(weightedStatBoosts)
        );
        return result;
    }
}