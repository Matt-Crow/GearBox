namespace GearBox.Core.Model.Dynamic;

using GearBox.Core.Model.Units;

/// <summary>
/// Rather than implementing a different interface, mobile dynamic objects 
/// should have an instance of this object.
/// </summary>
public class MobileBehavior
{
    private readonly BodyBehavior _body;

    public MobileBehavior(Velocity velocity) : this(new(), velocity)
    {

    }

    public MobileBehavior(BodyBehavior body, Velocity velocity)
    {
        _body = body;
        Velocity = velocity;
    }

    public Coordinates Coordinates { get => _body.Location; set => _body.Location = value; }
    public Velocity Velocity { get; set; }
    public bool IsMoving { get; set; } = false;

    public void StartMovingIn(Direction direction)
    {
        // might make these only accept cardinal directions later...
        if (IsMoving)
        {
            // already moving, check if we need to merge directions
            var oldDirection = Velocity.Angle;
            if (oldDirection.IsOrthogonalTo(direction))
            {
                var newDirection = Direction.Between(oldDirection, direction);
                Velocity = Velocity.InDirection(newDirection);
            }
            else if (oldDirection.IsOpposite(direction))
            {
                IsMoving = false;
            }
            else
            {
                Velocity = Velocity.InDirection(direction);
            }
        }
        else 
        {
            IsMoving = true;
            Velocity = Velocity.InDirection(direction);
        }
    }

    public void StopMovingIn(Direction direction)
    {
        var currentDirection = Velocity.Angle;
        if (currentDirection.Equals(direction))
        {
            IsMoving = false;
        }
        else if (direction.IsCardinal && Direction.DegreesBetween(currentDirection, direction) == 45)
        {
            var newDirection = Direction.UnBetween(currentDirection, direction);
            Velocity = Velocity.InDirection(newDirection);
        }
    }

    public void UpdateMovement()
    {
        if (IsMoving)
        {
            Coordinates = Coordinates.Plus(Velocity);
        }
    }
}