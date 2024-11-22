using System.Text.Json;
using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// A Character is something sentient in the game.
/// </summary>
public abstract class Character : IGameObject
{
    public static readonly int MAX_LEVEL = 20;
    protected static readonly Speed BASE_SPEED = Speed.FromTilesPerSecond(3);

    private readonly MobileBehavior _mobility;


    public Character(string name, int level, Color? color = null, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Color = color ?? Color.GREEN;
        _mobility = new MobileBehavior(Body, Velocity.FromPolar(BASE_SPEED, Direction.DOWN));
        Serializer = new(Type, Serialize);
        Termination = new(this, () => DamageTaken >= MaxHitPoints);
        BasicAttack = new BasicAttack()
        {
            User = this
        };
        SetLevel(level);
    }


    public Guid Id { get; init; }
    public string Name { get; init; }
    protected Color Color { get; init; }
    public Serializer Serializer { get; init; }
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
    public IArea? CurrentArea { get; private set; }
    public IArea? LastArea { get; private set; }
    public Team Team { get; set; } = new(); // default to each on their own team

    public event EventHandler<AttackedEventArgs>? Attacked;
    public event EventHandler<KilledEventArgs>? Killed;

    public void SetLevel(int level)
    {
        Level = level;

        // avoid virtual call in ctor: https://stackoverflow.com/q/119506
        UpdateStatsBase();
    }

    public virtual void SetArea(IArea? newArea)
    {
        if (CurrentArea != null)
        {
            LastArea = CurrentArea;
        }
        CurrentArea = newArea;
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

    public void UseBasicAttack(Direction inDirection)
    {
        BasicAttack.Use(inDirection);
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

    public void HandleAttacked(AttackedEventArgs attackEvent)
    {
        if (Termination.IsTerminated)
        {
            // do not resolve the event if dead
            return;
        }

        TakeDamage(attackEvent.AttackUsed.Damage);
        Attacked?.Invoke(this, attackEvent);

        if (Termination.IsTerminated)
        {
            var killedEvent = new KilledEventArgs(attackEvent);
            Killed?.Invoke(this, killedEvent);
        }
    }

    public virtual void Update()
    {
        _mobility.UpdateMovement();
        BasicAttack.Update();
    }

    protected virtual string Serialize(SerializationOptions options)
    {
        var json = new CharacterJson(
            Id,
            Name,
            Level,
            Color.ToJson(),
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