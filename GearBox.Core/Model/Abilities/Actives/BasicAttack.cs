using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Abilities.Actives;

public class BasicAttack
{
    private readonly Character _user;
    private int _cooldownInFrames = 0;

    public BasicAttack(Character user)
    {
        _user = user;
    }

    public AttackRange Range { get; set; } = AttackRange.MELEE;

    public void Use(World inWorld, Direction inDirection)
    {
        if (_cooldownInFrames != 0)
        {
            return;
        }

        var damage = GetDamagePerHitByLevel(_user.Level) * (1.0 + _user.DamageModifier);
        var attack = new Attack(_user, (int)damage);
        var projectile = new Projectile(
            _user.Coordinates, 
            Velocity.FromPolar(Speed.FromTilesPerSecond(Range.Range.InTiles), inDirection),
            Range.Range,
            attack
        );
        inWorld.GameObjects.AddGameObject(projectile);
        _cooldownInFrames = Duration.FromSeconds(0.5).InFrames;
    }

    private static int GetDamagePerHitByLevel(int level)
    {
        // ranges from 50 to 183
        var result = 43 + 7*level;
        return result;
    }

    public void Update()
    {
        _cooldownInFrames--;
        if (_cooldownInFrames < 0)
        {
            _cooldownInFrames = 0;
        }
    }
}