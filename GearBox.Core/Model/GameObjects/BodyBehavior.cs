using GearBox.Core.Model.Areas;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// Defines behavior for an object which physically exists within an area.
/// </summary>
public class BodyBehavior
{
    public BodyBehavior() : this(Distance.FromTiles(0.5), th => th != TileHeight.FLOOR)
    {

    }

    public BodyBehavior(Distance radius, Predicate<TileHeight> canCollideWith)
    {
        Radius = radius;
        CanCollideWith = canCollideWith;
    }

    /// <summary>
    /// How wide this object is
    /// </summary>
    public Distance Radius { get; init; } = Distance.FromTiles(0.5);

    /// <summary>
    /// Where this object is centered
    /// </summary>
    public Coordinates Location { get; set; } = Coordinates.ORIGIN.CenteredOnTile();

    public Predicate<TileHeight> CanCollideWith { get; init; }
    
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

    public event EventHandler<CollideEventArgs>? Collided;
    public event EventHandler<CollideWithMapEdgeEventArgs>? CollideWithMapEdge;
    public event EventHandler<CollideWithTileEventArgs>? CollideWithTile;

    public bool CollidesWith(BodyBehavior other)
    {
        if (other == this)
        {
            return false; // cannot collide with self
        }
        
        var withinX = RightInPixels >= other.LeftInPixels && LeftInPixels <= other.RightInPixels;
        var withinY = BottomInPixels >= other.TopInPixels && TopInPixels <= other.BottomInPixels;
        return withinX && withinY;
    }

    public bool IsWithin(Dimensions dimensions)
    {
        var withinX = LeftInPixels >= 0 && RightInPixels <= dimensions.WidthInPixels;
        var withinY = TopInPixels >= 0 && BottomInPixels <= dimensions.HeightInPixels;
        return withinX && withinY;
    }

    public void OnCollided(CollideEventArgs args)
    {
        Collided?.Invoke(this, args);
    }

    public void OnCollidedWithMapEdge(CollideWithMapEdgeEventArgs args)
    {
        // by default, keep in bounds
        if (LeftInPixels < 0)
        {
            LeftInPixels = 0;
        }
        else if (RightInPixels >= args.MapDimensions.WidthInPixels)
        {
            RightInPixels = args.MapDimensions.WidthInPixels;
        }
        if (TopInPixels < 0)
        {
            TopInPixels = 0;
        }
        else if (BottomInPixels >= args.MapDimensions.HeightInPixels)
        {
            BottomInPixels = args.MapDimensions.HeightInPixels;
        }
        
        CollideWithMapEdge?.Invoke(this, args);
    }

    public void OnCollidedWithTile(CollideWithTileEventArgs args)
    {
        // default to shoving out
        args.Tile.ShoveOut(this);
        
        CollideWithTile?.Invoke(this, args);
    }
}