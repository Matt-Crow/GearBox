namespace GearBox.Core.Model.Stable.Items;

public class WeaponStats
{
    // not sure how I feel about _damagePerHit being a weapon stat... feels like it should go on PlayerCharacter
    private readonly int _damagePerHit;

    public WeaponStats(int damagePerHit, PlayerStatBoosts playerStatBoosts)
    {
        _damagePerHit = damagePerHit;
        PlayerStatBoosts = playerStatBoosts;
    }
    
    public PlayerStatBoosts PlayerStatBoosts { get; init; }
}