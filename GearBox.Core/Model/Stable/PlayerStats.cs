namespace GearBox.Core.Model.Stable;

public class PlayerStats
{
    public PlayerStat<int> MaxHitPoints { get; init; } = PlayerStat<int>.Linear(1000, 5.0);
    public PlayerStat<int> MaxEnergy { get; init; } = PlayerStat<int>.Linear(100, 3.0);

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(MaxHitPoints.DynamicValues)
        .Concat(MaxEnergy.DynamicValues); 
    
    public void SetStatBoosts(IEnumerable<PlayerStatBoosts> boosts)
    {
        MaxHitPoints.Points = boosts.Sum(x => x.MaxHitPoints);
        MaxEnergy.Points = boosts.Sum(x => x.MaxEnergy);
    }
}