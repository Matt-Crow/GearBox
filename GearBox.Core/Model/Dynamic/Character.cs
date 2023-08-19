namespace GearBox.Core.Model.Dynamic;

using GearBox.Core.Model.Units;

/// <summary>
/// A Character is something sentient in the game world.
/// </summary>
public class Character : IDynamicGameObject
{
    private readonly MobileBehavior _mobility;

    public Character(Velocity velocity)
    {
        _mobility = new MobileBehavior(velocity);
    }

    public void Update()
    {
        _mobility.UpdateMovement();
    }
}