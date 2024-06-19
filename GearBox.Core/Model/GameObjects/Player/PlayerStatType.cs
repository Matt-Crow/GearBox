namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStatType
{
    private readonly string _name;
    private readonly Func<PlayerStats, PlayerStat> _getStatFrom;
    
    public static readonly PlayerStatType MAX_HIT_POINTS = new("HP", stats => stats.MaxHitPoints);
    public static readonly PlayerStatType MAX_ENERGY = new("Energy", stats => stats.MaxEnergy);
    public static readonly PlayerStatType OFFENSE = new("Offense", stats => stats.Offense);
    public static readonly PlayerStatType SPEED = new("Speed", stats => stats.Speed);

    public static readonly IEnumerable<PlayerStatType> ALL = [
        MAX_HIT_POINTS,
        MAX_ENERGY,
        OFFENSE,
        SPEED
    ];

    private PlayerStatType(string name, Func<PlayerStats, PlayerStat> something)
    {
        _name = name;
        _getStatFrom = something;
    }

    public PlayerStat GetStatFrom(PlayerStats stats) => _getStatFrom(stats);

    public override string ToString() => _name;
}