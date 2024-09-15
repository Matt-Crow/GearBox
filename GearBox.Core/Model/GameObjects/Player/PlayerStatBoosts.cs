namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatBoosts
{
    private readonly Dictionary<PlayerStatType, int> _weights;
    private readonly Dictionary<PlayerStatType, int> _values = [];
    private readonly List<string> _details = [];

    public PlayerStatBoosts(Dictionary<PlayerStatType, int>? weights=null, int totalPoints=0)
    {
        weights ??= [];
        _weights = weights;
        var totalWeights = weights.Values.Sum();
        if (totalWeights == 0)
        {
            totalWeights = 1; // prevent divide by 0s
        }
        foreach (var statType in PlayerStatType.ALL)
        {
            var points = 0;
            if (weights.TryGetValue(statType, out int weightForStat))
            {
                points = weightForStat * totalPoints / totalWeights;
            }
            _values[statType] = points;
            if (points != 0)
            {
                _details.Add($"+{points} {statType}");
            }
        }
    }

    public static PlayerStatBoosts Empty()
    {
        return new PlayerStatBoosts([], 0);
    }

    /// <summary>
    /// Returns a copy of this, but with the stat boosts from other added
    /// </summary>
    public PlayerStatBoosts Combine(PlayerStatBoosts? other)
    {
        if (other == null)
        {
            return this;
        }

        var copyOfValues = new Dictionary<PlayerStatType, int>(_values);
        foreach (var statType in PlayerStatType.ALL)
        {
            copyOfValues[statType] = Get(statType) + other.Get(statType);
        }
        var totalPoints = copyOfValues.Values.Sum();

        return new PlayerStatBoosts(copyOfValues, totalPoints);
    }

    public int Get(PlayerStatType type) => _values[type];

    public IEnumerable<string> Details => _details;

    public PlayerStatBoosts WithTotalPoints(int totalPoints) => new PlayerStatBoosts(_weights, totalPoints);
}