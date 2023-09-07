namespace GearBox.Core.Model.Dynamic;

using GearBox.Core.Model.Units;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IDynamicGameObject
{
    private readonly MobileBehavior _mobility;

    public Character() : this(Velocity.FromPolar(Speed.InTilesPerSecond(3), Direction.DOWN))
    {

    }

    public Character(Velocity velocity)
    {
        _mobility = new MobileBehavior(velocity);
    }


    /// <summary>
    /// Used by clients to uniquely identify a character across updates.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    public Coordinates Coordinates { get => _mobility.Coordinates; set => _mobility.Coordinates = value; }


    public void StartMovingIn(Direction direction)
    {
        _mobility.StartMovingIn(direction);
    }

    public void StopMovingIn(Direction direction)
    {
        _mobility.StopMovingIn(direction);
    }

    public IDynamicGameObjectJson ToJson()
    {
        return new CharacterJson(
            Id,
            _mobility.Coordinates.XInPixels, 
            _mobility.Coordinates.YInPixels
        );
    }

    public void Update()
    {
        _mobility.UpdateMovement();
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