namespace GearBox.Core.Model.Dynamic;

public class Velocity
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

    public double ChangeInXInPixels { get => Magnitude.InPixelsPerFrame * Angle.XMultiplier; }
    public double ChangeInYInPixels { get => Magnitude.InPixelsPerFrame * Angle.YMultiplier; }
}