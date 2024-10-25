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
        var attack = new Attack(user, GetDamageWhenUsedBy(user)); 
        OnUse(user, inDirection, attack);
        _framesUntilNextUse = Cooldown.InFrames;
        if (user is PlayerCharacter player)
        {
            player.LoseEnergy(EnergyCost);
        }
    }

    protected abstract void OnUse(Character user, Direction inDirection, Attack attack);

    protected void SpawnProjectile(Character user, Direction inDirection, Attack attack, AttackRange range)
    {
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

    public abstract string GetDescription(Character character);

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
    protected virtual int GetDamageWhenUsedBy(Character user)
    {
        // ranges from 50 to 183
        var result = 43.0 + 7*user.Level;
        result = result*(1.0 + user.DamageModifier);
        return (int)result;
    }
}
