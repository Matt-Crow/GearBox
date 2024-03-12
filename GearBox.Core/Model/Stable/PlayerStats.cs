namespace GearBox.Core.Model.Stable;

public class PlayerStats
{
    public PlayerStat<int> MaxHitPoints { get; init; } = PlayerStat<int>.Linear(1000, 5.0);

    public IEnumerable<object?> DynamicValues => MaxHitPoints.DynamicValues; 
}