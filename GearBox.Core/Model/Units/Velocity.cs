namespace GearBox.Core.Model.Units;

public readonly struct Velocity
{
    private Velocity(Speed magnitude, Direction angle)
    {
        Magnitude = magnitude;
        Angle = angle;
    }

    public static Velocity FromPolar(Speed magnitude, Direction angle)
    {
        return new Velocity(magnitude, angle);
    }


    public Speed Magnitude { get; init; }
    public Direction Angle { get; init; }

    /// <summary>
    /// Creates a readonly copy of this, except facing the given direction.
    /// </summary>
    /// <param name="direction">the new direction</param>
    /// <returns>a readonly copy of this, except in the given direction</returns>
    public Velocity InDirection(Direction direction)
    {
        return new Velocity(Magnitude, direction);
    }

    public double ChangeInXInPixels { get => Magnitude.InPixelsPerFrame * Angle.XMultiplier; }
    public double ChangeInYInPixels { get => Magnitude.InPixelsPerFrame * Angle.YMultiplier; }
}