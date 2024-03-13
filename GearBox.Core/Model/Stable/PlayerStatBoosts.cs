namespace GearBox.Core.Model.Stable;

public class PlayerStatBoosts
{
    private readonly Dictionary<PlayerStatType, int> _values = new();

    public PlayerStatBoosts()
    {
        foreach (var statType in PlayerStatType.ALL)
        {
            _values[statType] = 0;
        }
    }

    public PlayerStatBoosts Add(PlayerStatType type, int points)
    {
        // while I don't like mutability, seems like a huge waste of memory to copy this class on modify
        _values[type] += points;
        return this;
    }

    public PlayerStatBoosts Add(PlayerStatBoosts? other)
    {
        if (other == null)
        {
            return this;
        }
        
        foreach (var statType in PlayerStatType.ALL)
        {
            Add(statType, other.Get(statType));
        }
        return this;
    }

    public int Get(PlayerStatType type) => _values[type];
}