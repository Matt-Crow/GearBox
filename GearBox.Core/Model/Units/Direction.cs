namespace GearBox.Core.Model.Units;

public readonly struct Direction
{
    private readonly int _bearingInDegrees;
    private readonly double _bearingInRadians;

    public static readonly Direction UP = FromBearingDegrees(0);
    public static readonly Direction RIGHT = FromBearingDegrees(90);
    public static readonly Direction DOWN = FromBearingDegrees(180);
    public static readonly Direction LEFT = FromBearingDegrees(270);


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

    public static int DegreesBetween(Direction a, Direction b)
    {
        // make life easier by ensuring a < b
        if (a._bearingInDegrees > b._bearingInDegrees)
        {
            return DegreesBetween(b, a);
        }
        
        var degrees = b._bearingInDegrees - a._bearingInDegrees;

        /*
            You can get from one angle to another by going either clockwise or
            counterclockwise; make sure the previous calculation didn't go the
            long way!
        */
        if (degrees > 180) {
            degrees = 360 - degrees;
        }

        return degrees;
    }

    /// <summary>
    /// Creates a new direction between the given 2 directions
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Direction Between(Direction a, Direction b)
    {
        /*
            Suppose there exist 3 angles: a, b, and c.
            Where c is located between a & b
                a < c < b OR b < c < a
            and c is an equidistant from both a & b
                |a - c| = |b - c|
            AND the angle between a & c <= 90 degrees
            solve for c.

            2 cases:
            if a < c < b
                then |a - c| = c - a
                and |b - c| = b - c = c - a
                thus c = (a+b)/2
            if b < c < a
                then |a - c| = a - c
                and |b - c| = c - b = a - c
                thus c = (a+b)/2
        */

        var degrees = (a._bearingInDegrees + b._bearingInDegrees)/2;

        /*
            There are always 2 angles between a & b and of equal distance from
            each of them. For example, for a = 0 and b = 90, both 45 (c) and 225
            (c') satisfy those requirements, so we need to check whether we've
            found c or c'
        */
        if (Math.Abs(a._bearingInDegrees - degrees) > 90)
        {
            // we've found c', so convert to c
            degrees -= 180;
        }

        return FromBearingDegrees(degrees);
    }

    /// <summary>
    /// Undoes the effect of Between such that if
    /// c = Between(a, b)
    /// then
    /// b = UnBetween(c, a)
    /// </summary>
    /// <param name="c">a diagonal direction</param>
    /// <param name="a">a cardinal direction</param>
    /// <returns></returns>
    public static Direction UnBetween(Direction c, Direction a)
    {
        /*
            Suppose there exist 3 angles: a, b, and c.
            Where c is located between a & b
                a < c < b OR b < c < a
            and c is an equidistant from both a & b
                |a - c| = |b - c|
            solve for b.

            2 cases:
            if a < c < b
                then |a - c| = c - a
                and |b - c| = b - c = c - a
                thus b = 2c - a
            if b < c < a
                then |a - c| = a - c
                and |b - c| = c - b = a - c
                thus b = 2c - a
        */
        var degrees = 2*c._bearingInDegrees - a._bearingInDegrees;
        return FromBearingDegrees(degrees);
    }

    public static Direction FromAToB(Coordinates a, Coordinates b)
    {
        var dx = b.XInPixels - a.XInPixels;
        var dy = b.YInPixels - a.YInPixels;
        var thetaInRadians = Math.Atan2(-dy, dx);
        var thetaInDegrees = 180 * thetaInRadians / Math.PI;
        var bearingDegrees = 90 - thetaInDegrees;
        return FromBearingDegrees((int)bearingDegrees);
    }

    public double XMultiplier { get => Math.Cos(Math.PI/2 - _bearingInRadians); }
    public double YMultiplier { get => -Math.Sin(Math.PI/2 - _bearingInRadians); }
    public bool IsCardinal { get => _bearingInDegrees % 90 == 0; }

    public bool IsOrthogonalTo(Direction other)
    {
        var dotProduct = other.XMultiplier*XMultiplier + other.YMultiplier*YMultiplier;
        return Math.Round(dotProduct, 6) == 0;
    }

    public bool IsOpposite(Direction other)
    {
        var sumX = other.XMultiplier + XMultiplier;
        var sumY = other.YMultiplier + YMultiplier;
        return Math.Round(sumX, 3) == 0 && Math.Round(sumY, 3) == 0;
    }

    public override string ToString()
    {
        return $"[{Math.Round(XMultiplier, 3)}, {Math.Round(YMultiplier, 3)}]";
    }
}