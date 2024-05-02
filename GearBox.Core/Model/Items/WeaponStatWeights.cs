using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Items;

public class WeaponStatWeights
{
    private readonly Dictionary<PlayerStatType, int> _playerStatWeights = new();

    public WeaponStatWeights()
    {
        foreach (var playerStatType in PlayerStatType.ALL)
        {
            _playerStatWeights[playerStatType] = 0;
        }
    }

    public WeaponStatWeights Weigh(PlayerStatType playerStatType, int weight)
    {
        _playerStatWeights[playerStatType] = weight;
        return this;
    }

    public PlayerStatBoosts Build(int totalPoints)
    {
        var totalWeights = _playerStatWeights.Values.Sum();
        if (totalWeights == 0)
        {
            totalWeights = 1; // prevent divide by 0
        }

        var weightedStatBoosts = _playerStatWeights.ToDictionary(
            x => x.Key,
            x => x.Value * totalPoints / totalWeights
        );

        var result = new PlayerStatBoosts(weightedStatBoosts);
        return result;
    }
}