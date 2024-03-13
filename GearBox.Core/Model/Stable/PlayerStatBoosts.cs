namespace GearBox.Core.Model.Stable;

public readonly struct PlayerStatBoosts
{
    private PlayerStatBoosts(int maxHitPoints, int maxEnergy)
    {
        MaxHitPoints = maxHitPoints;
        MaxEnergy = maxEnergy;
    }

    public int MaxHitPoints { get; init; }
    public int MaxEnergy { get; init; }

    public class Builder
    {
        private readonly int _maxHitPoints = 0;
        private readonly int _maxEnergy = 0;

        private Builder(int maxHitPoints, int maxEnergy)
        {
            _maxHitPoints = maxHitPoints;
            _maxEnergy = maxEnergy;
        }

        public Builder() : this(0, 0)
        {

        }

        public Builder WithMaxHitPoints(int maxHitPoints) => new(maxHitPoints, _maxEnergy);
        public Builder WithMaxEnergy(int maxEnergy) => new(_maxHitPoints, maxEnergy);

        public PlayerStatBoosts Build() => new(_maxHitPoints, _maxEnergy);
    }
}