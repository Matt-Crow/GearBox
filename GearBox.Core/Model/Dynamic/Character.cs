namespace GearBox.Core.Model.Dynamic;

using GearBox.Core.Model.Units;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IDynamicGameObject
{
    private readonly MobileBehavior _mobility;

    public Character() : this(Velocity.FromPolar(Speed.InTilesPerSecond(5), Direction.DOWN))
    {

    }

    public Character(Velocity velocity)
    {
        _mobility = new MobileBehavior(velocity);
    }


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
            _mobility.Coordinates.XInPixels, 
            _mobility.Coordinates.YInPixels
        );
    }

    public void Update()
    {
        _mobility.UpdateMovement();
    }
}