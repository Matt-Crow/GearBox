using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IDynamicGameObject
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
    }


    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }
    public Serializer Serializer { get; init; }
    public BodyBehavior Body { get; init; } = new();
    public TerminateBehavior Termination { get; init; }
    public Coordinates Coordinates { get => Body.Location; set => Body.Location = value; }
    public int DamageTaken {get; private set; } = 0; // track damage taken instead of remaining HP to avoid issues when swapping armor
    public int MaxHitPoints { get; set; }
    protected virtual string Type => "character";
    protected int Level { get; private set; }
    protected int DamagePerHit { get; private set; } // tie DPH to character instead of weapon so unarmed works


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
        DamagePerHit = GetDamagePerHitByLevel(Level);
    }

    public void StartMovingIn(Direction direction)
    {
        _mobility.StartMovingIn(direction);
    }

    public void StopMovingIn(Direction direction)
    {
        _mobility.StopMovingIn(direction);
    }

    public void SetSpeed(Speed speed)
    {
        _mobility.Velocity = _mobility.Velocity.WithSpeed(speed);
    }

    public virtual void TakeDamage(int damage)
    {
        DamageTaken += damage;
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
        _mobility.UpdateMovement();
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

    private int GetDamagePerHitByLevel(int level)
    {
        // ranges from 50 to 183
        var result = 43 + 7*level;
        return result;
    }
}