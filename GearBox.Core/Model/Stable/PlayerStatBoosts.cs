namespace GearBox.Core.Model.Stable;

public class PlayerStatBoosts
{
    private readonly Dictionary<PlayerStatType, int> _values = new();

    public PlayerStatBoosts(Dictionary<PlayerStatType, int>? values = null)
    {
        values ??= new();
        foreach (var statType in PlayerStatType.ALL)
        {
            _values[statType] = (values.ContainsKey(statType)) ? values[statType] : 0;
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
}