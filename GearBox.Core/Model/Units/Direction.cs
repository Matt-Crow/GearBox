namespace GearBox.Core.Model.Units;

public class Direction
{
    private readonly int _bearingInDegrees;
    private readonly double _bearingInRadians;

    public static readonly Direction UP = Direction.FromBearingDegrees(0);
    public static readonly Direction RIGHT = Direction.FromBearingDegrees(90);
    public static readonly Direction DOWN = Direction.FromBearingDegrees(180);
    public static readonly Direction LEFT = Direction.FromBearingDegrees(270);


    private Direction(int bearingInDegrees)
    {
        while (bearingInDegrees < 0)
        {
            bearingInDegrees += 360;
        }
        _bearingInDegrees = bearingInDegrees % 360;
        _bearingInRadians = Math.PI * _bearingInDegrees / 180;
    }

    public static Direction FromBearingDegrees(int degrees)
    {
        return new Direction(degrees);
    }


    public double XMultiplier { get => Math.Cos(Math.PI/2 - _bearingInRadians); }
    public double YMultiplier { get => -Math.Sin(Math.PI/2 - _bearingInRadians); }
}