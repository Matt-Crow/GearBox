namespace GearBox.Core.Model.Stable;

public readonly struct PlayerStatBoosts
{
    private PlayerStatBoosts(int maxHitPoints, int maxEnergy, int offense, int defense, int speed)
    {
        MaxHitPoints = maxHitPoints;
        MaxEnergy = maxEnergy;
        Offense = offense;
        Defense = defense;
        Speed = speed;
    }

    public int MaxHitPoints { get; init; }
    public int MaxEnergy { get; init; }
    public int Offense { get; init; }
    public int Defense { get; init; }
    public int Speed { get; init; }

    public class Builder
    {
        private readonly Dictionary<PlayerStatType, int> _stats;

        public Builder(Dictionary<PlayerStatType, int>? stats = null)
        {
            _stats = stats ?? new();
        }

        public Builder WithMaxHitPoints(int maxHitPoints) => Add(PlayerStatType.MaxHitPoints, maxHitPoints);
        public Builder WithMaxEnergy(int maxEnergy) => Add(PlayerStatType.MaxEnergy, maxEnergy);
        public Builder WithOffense(int offense) => Add(PlayerStatType.Offense, offense);
        public Builder WithDefense(int defense) => Add(PlayerStatType.Defense, defense);
        public Builder WithSpeed(int speed) => Add(PlayerStatType.Speed, speed);

        private Builder Add(PlayerStatType type, int points)
        {
            var copyOfStats =  new Dictionary<PlayerStatType, int>(_stats);
            copyOfStats[type] = points;
            return new Builder(copyOfStats);
        }

        private int Get(PlayerStatType type) => _stats.ContainsKey(type) ? _stats[type] : 0;

        public PlayerStatBoosts Build() => new(
            Get(PlayerStatType.MaxHitPoints), 
            Get(PlayerStatType.MaxEnergy), 
            Get(PlayerStatType.Offense), 
            Get(PlayerStatType.Defense),
            Get(PlayerStatType.Speed)
        );
    }
}