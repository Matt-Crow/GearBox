namespace GearBox.Core.Model.Dynamic.Player;

public class PlayerStats
{
    public PlayerStat<double> MaxHitPoints { get; init; } = PlayerStat<double>.DiminishingReturn();
    public PlayerStat<double> MaxEnergy { get; init; } = PlayerStat<double>.DiminishingReturn();
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