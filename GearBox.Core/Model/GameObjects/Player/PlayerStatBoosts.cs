namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatBoosts
{
    private readonly Dictionary<PlayerStatType, int> _values = new();
    private readonly List<string> _details = [];

    public PlayerStatBoosts(Dictionary<PlayerStatType, int>? values = null)
    {
        values ??= [];
        foreach (var statType in PlayerStatType.ALL)
        {
            var points = 0;
            if (values.TryGetValue(statType, out int value))
            {
                points = value;
            }
            _values[statType] = points;
            if (points != 0)
            {
                _details.Add($"+{values[statType]} {statType}");
            }
        }
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

        return new PlayerStatBoosts(copyOfValues);
    }

    public int Get(PlayerStatType type) => _values[type];

    public IEnumerable<string> Details => _details;
}