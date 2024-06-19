namespace GearBox.Core.Model.GameObjects.Player;

public class PlayerStats
{
    public PlayerStat MaxHitPoints { get; init; } = new PlayerStat();
    public PlayerStat MaxEnergy { get; init; } = new PlayerStat();
    public PlayerStat Offense { get; init; } = new PlayerStat();
    public PlayerStat Speed { get; init; } = new PlayerStat();

    public IEnumerable<object?> DynamicValues => Array.Empty<object?>()
        .Concat(MaxHitPoints.DynamicValues)
        .Concat(MaxEnergy.DynamicValues)
        .Concat(Offense.DynamicValues)
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