using System.Text.Json;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IDynamicGameObject
{
    private readonly MobileBehavior _mobility;
    

    public Character() : this(Velocity.FromPolar(Speed.FromTilesPerSecond(3), Direction.DOWN))
    {

    }

    public Character(Velocity velocity)
    {
        _mobility = new MobileBehavior(Body, velocity);
    }

    public virtual string Type => "character";
    
    /// <summary>
    /// Used by clients to uniquely identify a character across updates.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    public BodyBehavior Body { get; init; } = new();

    public bool IsTerminated => DamageTaken >= MaxHitPoints;

    public Coordinates Coordinates { get => Body.Location; set => Body.Location = value; }
    
    public int DamageTaken {get; private set; } = 0; // track damage taken instead of remaining HP to avoid issues when swapping armor
    
    public int MaxHitPoints { get; set; }


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

    public virtual string Serialize(JsonSerializerOptions options)
    {
        var json = new CharacterJson(
            Id,
            _mobility.Coordinates.XInPixels, 
            _mobility.Coordinates.YInPixels,
            new FractionJson(MaxHitPoints - DamageTaken, MaxHitPoints)
        );
        return JsonSerializer.Serialize(json, options);
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
}