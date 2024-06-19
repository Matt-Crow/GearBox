namespace GearBox.Core.Model.Static;

/// <summary>
/// Multiton for wall, floor, or pit
/// </summary>
public class TileHeight
{
    /// <summary>
    /// Blocks characters and projectiles
    /// </summary>
    public static readonly TileHeight WALL = new("Wall", 1);
    
    /// <summary>
    /// Does not block anything
    /// </summary>
    public static readonly TileHeight FLOOR = new("Floor", 0);
    
    /// <summary>
    /// Blocks characters
    /// </summary>
    public static readonly TileHeight PIT = new("Pit", -1);

    private TileHeight(string name, int height)
    {
        Name = name;
        Height = height;
    }

    /// <summary>
    /// String representation of this height
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Compare this to 0
    /// </summary>
    public int Height { get; init; }

    public override string ToString()
    {
        return Name;
    }
}