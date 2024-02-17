using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// Defines behavior for an object which physically exists within a world.
/// </summary>
public class BodyBehavior
{
    public BodyBehavior() : this(Distance.FromTiles(0.5))
    {

    }

    public BodyBehavior(Distance radius)
    {
        Radius = radius;
    }

    /// <summary>
    /// How wide this object is
    /// </summary>
    public Distance Radius { get; init; } = Distance.FromTiles(0.5);

    /// <summary>
    /// Where this object is centered
    /// </summary>
    public Coordinates Location { get; set; } = Coordinates.ORIGIN.CenteredOnTile();

    public int LeftInPixels
    {
        get => Location.XInPixels - Radius.InPixels;
        set => Location = Coordinates.FromPixels(value + Radius.InPixels, Location.YInPixels);
    }

    public int RightInPixels
    {
        get => Location.XInPixels + Radius.InPixels;
        set => Location = Coordinates.FromPixels(value - Radius.InPixels, Location.YInPixels);
    }

    public int TopInPixels
    {
        get => Location.YInPixels - Radius.InPixels;
        set => Location = Coordinates.FromPixels(Location.XInPixels, value + Radius.InPixels);
    }

    public int BottomInPixels
    {
        get => Location.YInPixels + Radius.InPixels;
        set => Location = Coordinates.FromPixels(Location.XInPixels, value - Radius.InPixels);
    }

    public bool CollidesWith(BodyBehavior other)
    {
        var withinX = RightInPixels >= other.LeftInPixels && LeftInPixels <= other.RightInPixels;
        var withinY = BottomInPixels >= other.TopInPixels && TopInPixels <= other.BottomInPixels;
        return withinX && withinY;
    }
}