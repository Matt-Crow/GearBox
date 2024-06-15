using System.Text.Json;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.GameObjects.Ai;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IGameObject
{
    public static readonly int MAX_LEVEL = 20;
    protected static readonly Speed BASE_SPEED = Speed.FromTilesPerSecond(3);

    private readonly MobileBehavior _mobility;


    public Character(string name, int level)
    {
        Name = name;
        _mobility = new MobileBehavior(Body, Velocity.FromPolar(BASE_SPEED, Direction.DOWN));
        Serializer = new(Type, Serialize);
        Termination = new(this, () => DamageTaken >= MaxHitPoints);
        SetLevel(level);
        BasicAttack = new(this);
    }


    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }
    public Serializer Serializer { get; init; }
    public IAiBehavior AiBehavior { get; set; } = new NullAiBehavior();
    public BodyBehavior Body { get; init; } = new();
    public TerminateBehavior Termination { get; init; }
    public Coordinates Coordinates { get => Body.Location; set => Body.Location = value; }
    public int DamageTaken {get; private set; } = 0; // track damage taken instead of remaining HP to avoid issues when swapping armor
    public int MaxHitPoints { get; set; }
    public int HitPointsRemaining => MaxHitPoints - DamageTaken;
    protected virtual string Type => "character";
    public int Level { get; private set; }
    public double DamageModifier { get; protected set; } = 0.0;
    public ArmorClass ArmorClass { get; protected set; } = ArmorClass.NONE;
    public BasicAttack BasicAttack { get; init; }
    public World? World { get; set; }
    public Team Team { get; set; } = new(); // default to each on their own team

    public void SetLevel(int level)
    {
        Level = level;

        // avoid virtual call in ctor: https://stackoverflow.com/q/119506
        UpdateStatsBase();
    }

    public virtual void UpdateStats()
    {
        UpdateStatsBase();
    }

    private void UpdateStatsBase()
    {
        MaxHitPoints = GetMaxHitPointsByLevel(Level);
    }

    public void StartMovingIn(Direction direction)
    {
        _mobility.StartMovingIn(direction);
    }

    public void StopMovingIn(Direction direction)
    {
        _mobility.StopMovingIn(direction);
    }

    public void StopMoving()
    {
        _mobility.StopMoving();
    }

    public void SetSpeed(Speed speed)
    {
        _mobility.Velocity = _mobility.Velocity.WithSpeed(speed);
    }

    public void UseBasicAttack(World inWorld, Direction inDirection)
    {
        BasicAttack.Use(inWorld, inDirection);
    }

    public void TakeDamage(int damage)
    {
        DamageTaken += ArmorClass.ReduceDamage(damage);
        if (DamageTaken > MaxHitPoints)
        {
            DamageTaken = MaxHitPoints;
        }
    }

    public void HealPercent(double percent)
    {
        DamageTaken -= (int)(MaxHitPoints*percent);
        if (DamageTaken < 0)
        {
            DamageTaken = 0;
        }
    }

    public virtual void Update()
    {
        AiBehavior.Update();
        _mobility.UpdateMovement();
        BasicAttack.Update();
    }

    protected virtual string Serialize(SerializationOptions options)
    {
        var json = new CharacterJson(
            Id,
            Name,
            Level,
            _mobility.Coordinates.XInPixels, 
            _mobility.Coordinates.YInPixels,
            new FractionJson(MaxHitPoints - DamageTaken, MaxHitPoints)
        );
        return JsonSerializer.Serialize(json, options.JsonSerializerOptions);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Character other)
        {
            return false;
        }
        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected virtual int GetMaxHitPointsByLevel(int level)
    {
        // ranges from 120 to 500
        var result = 100 + 20*level;
        return result;
    }
}