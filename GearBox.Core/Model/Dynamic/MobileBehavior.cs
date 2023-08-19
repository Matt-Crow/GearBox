namespace GearBox.Core.Model.Dynamic;

using GearBox.Core.Model.Units;

/// <summary>
/// Rather than implementing a different interface, mobile dynamic objects 
/// should have an instance of this object.
/// </summary>
public class MobileBehavior
{
    public MobileBehavior(Velocity velocity)
    {
        Velocity = velocity;
    }

    public Coordinates Coordinates { get; set; } = Coordinates.ORIGIN;
    public Velocity Velocity { get; set; }
    public bool IsMoving { get; set; } = false;

    public void UpdateMovement()
    {
        if (IsMoving)
        {
            Coordinates = Coordinates.Plus(Velocity);
        }
    }
}