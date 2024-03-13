namespace GearBox.Core.Model.Stable;

public class PlayerStats
{
    public PlayerStat<int> MaxHitPoints { get; init; } = PlayerStat<int>.Linear(1000, 5.0);
    public PlayerStat<int> MaxEnergy { get; init; } = PlayerStat<int>.Linear(100, 3.0);
    public PlayerStat<double> Offense { get; init; } = PlayerStat<double>.DiminishingReturn();
    public PlayerStat<double> Defense { get; init; } = PlayerStat<double>.DiminishingReturn();
    public PlayerStat<double> Speed { get; init; } = PlayerStat<double>.DiminishingReturn();

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(MaxHitPoints.DynamicValues)
        .Concat(MaxEnergy.DynamicValues)
        .Concat(Offense.DynamicValues)
        .Concat(Defense.DynamicValues)
        .Concat(Speed.DynamicValues); 
    
    public void SetStatBoosts(PlayerStatBoosts boosts)
    {
        foreach (var statType in PlayerStatType.ALL)
        {
            var statToBoost = statType.GetStatFrom(this);
            statToBoost.Points = boosts.Get(statType);
        }
    }
}