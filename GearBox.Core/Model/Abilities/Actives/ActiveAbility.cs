using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives;

public abstract class ActiveAbility : IActiveAbility
{
    private int _framesUntilNextUse = 0;

    public ActiveAbility(int energyCost, Duration cooldown)
    {
        EnergyCost = energyCost;
        Cooldown = cooldown;
    }

    public int EnergyCost { get; init; }
    public Duration Cooldown { get; init; }
    public Duration TimeUntilNextUse => Duration.FromFrames(_framesUntilNextUse);

    public bool CanBeUsedBy(Character character)
    {
        if (_framesUntilNextUse > 0)
        {
            return false;
        }
        if (character is PlayerCharacter player && player.EnergyRemaining < EnergyCost)
        {
            return false;
        }
        return true;
    }

    public void Use(Character user, Direction inDirection)
    {
        if (!CanBeUsedBy(user))
        {
            return;
        }
        OnUse(user, inDirection);
        _framesUntilNextUse = Cooldown.InFrames;
        if (user is PlayerCharacter player)
        {
            player.LoseEnergy(EnergyCost);
        }
    }

    protected abstract void OnUse(Character user, Direction inDirection);

    protected void SpawnProjectile(Character user, Direction inDirection, AttackRange range, double damageMultiplier)
    {
        var inArea = user.CurrentArea ?? throw new Exception("Cannot use basic attack when not in an area");
        
        var damage = GetBaseDamagePerHitByLevel(user.Level) * (1.0 + user.DamageModifier) * damageMultiplier;
        
        // todo pull up in case multiple projectiles
        var attack = new Attack(user, (int)damage); 
        
        var projectile = new Projectile(
            user.Coordinates, 
            Velocity.FromPolar(Speed.FromTilesPerSecond(range.Range.InTiles), inDirection),
            range.Range,
            attack,
            range.ProjectileColor
        );
        
        inArea.AddProjectile(projectile);
    }

    public void Update()
    {
        _framesUntilNextUse--;
        if (_framesUntilNextUse < 0)
        {
            _framesUntilNextUse = 0;
        }
    }

    /// <summary>
    /// Helper method - only needed for damaging active abilities
    /// </summary>
    protected static int GetBaseDamagePerHitByLevel(int level)
    {
        // ranges from 50 to 183
        var result = 43 + 7*level;
        return result;
    }
}
