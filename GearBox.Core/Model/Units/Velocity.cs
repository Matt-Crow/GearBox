namespace GearBox.Core.Model.Units;

public readonly struct Velocity
{
    public static readonly Velocity ZERO = new(Speed.FromTilesPerSecond(0), Direction.DOWN);
    
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
    /// Creates a readonly copy of this, except with the given speed.
    /// </summary>
    /// <param name="speed">the new speed</param>
    /// <returns>a readonly copy of this, except in the given direction</returns>
    public Velocity WithSpeed(Speed speed) => new(speed, Angle);

    /// <summary>
    /// Creates a readonly copy of this, except facing the given direction.
    /// </summary>
    /// <param name="direction">the new direction</param>
    /// <returns>a readonly copy of this, except in the given direction</returns>
    public Velocity InDirection(Direction direction) => new(Magnitude, direction);

    public double ChangeInXInPixels => Magnitude.InPixelsPerFrame * Angle.XMultiplier;
    public double ChangeInYInPixels => Magnitude.InPixelsPerFrame * Angle.YMultiplier;
}