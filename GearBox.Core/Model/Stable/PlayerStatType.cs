using System.Collections.Immutable;

namespace GearBox.Core.Model.Stable;

public class PlayerStatType
{
    private readonly Func<PlayerStats, IPlayerStat> _getStatFrom;
    
    public static readonly PlayerStatType MAX_HIT_POINTS = new(stats => stats.MaxHitPoints);
    public static readonly PlayerStatType MAX_ENERGY = new(stats => stats.MaxEnergy);
    public static readonly PlayerStatType OFFENSE = new(stats => stats.Offense);
    public static readonly PlayerStatType DEFENSE = new(stats => stats.Defense);
    public static readonly PlayerStatType SPEED = new(stats => stats.Speed);

    public static readonly IEnumerable<PlayerStatType> ALL = ImmutableArray.Create<PlayerStatType>([
        MAX_HIT_POINTS,
        MAX_ENERGY,
        OFFENSE,
        DEFENSE,
        SPEED
    ]);

    private PlayerStatType(Func<PlayerStats, IPlayerStat> something)
    {
        _getStatFrom = something;
    }

    public IPlayerStat GetStatFrom(PlayerStats stats) => _getStatFrom(stats);
}