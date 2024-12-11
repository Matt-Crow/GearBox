using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives;

public abstract class ActiveAbility : IActiveAbility
{
    private int _framesUntilNextUse = 0;

    public ActiveAbility(string name, int energyCost, Duration cooldown)
    {
        Name = name;
        EnergyCost = energyCost;
        Cooldown = cooldown;
    }

    public string Name { get; init; }
    public int EnergyCost { get; init; }
    public Duration Cooldown { get; init; }
    public Character? User { get; set; }
    public Duration TimeUntilNextUse => Duration.FromFrames(_framesUntilNextUse);

    public bool CanBeUsed()
    {
        if (_framesUntilNextUse > 0)
        {
            return false;
        }
        if (User is PlayerCharacter player && player.EnergyRemaining < EnergyCost)
        {
            return false;
        }
        return true;
    }

    public void Use(Direction inDirection)
    {
        if (!CanBeUsed())
        {
            return;
        }
        var user = User ?? throw new Exception("Cannot use if User isn't set");
        var attack = new Attack(user, GetDamage()); 
        OnUse(inDirection, attack);
        _framesUntilNextUse = Cooldown.InFrames;
        if (user is PlayerCharacter player)
        {
            player.LoseEnergy(EnergyCost);
        }
    }

    protected abstract void OnUse(Direction inDirection, Attack attack);

    protected void SpawnProjectile(Direction inDirection, Attack attack, AttackRange range)
    {
        var user = User ?? throw new Exception("Cannot spawn projectile when user isn't set");
        var inArea = user.CurrentArea ?? throw new Exception("Cannot spawn projectile when not in an area");

        var projectile = new Projectile(
            user.Coordinates, 
            Velocity.FromPolar(Speed.FromTilesPerSecond(range.Range.InTiles), inDirection),
            range.Range,
            attack,
            range.ProjectileColor
        );
        
        inArea.AddProjectile(projectile);
    }

    public abstract string GetDescription();

    public void Update()
    {
        _framesUntilNextUse--;
        if (_framesUntilNextUse < 0)
        {
            _framesUntilNextUse = 0;
        }
    }

    public abstract IActiveAbility Copy();

    /// <summary>
    /// Helper method - only needed for damaging active abilities
    /// </summary>
    protected virtual int GetDamage()
    {
        var level = User?.Level ?? 1;
        var damageModifier = User?.DamageModifier ?? 0.0;

        // ranges from 50 to 183
        var result = 43.0 + 7*level;
        result = result*(1.0 + damageModifier);
        return (int)result;
    }
}
