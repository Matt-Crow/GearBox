using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Areas;

public class Tile
{
    public Tile(Coordinates upperLeft, TileType tileType)
    {
        UpperLeft = upperLeft;
        TileType = tileType;
    }

    public Coordinates UpperLeft { get; init; }
    public TileType TileType { get; init; }

    public int TopInPixels { get => UpperLeft.YInPixels; }
    public int BottomInPixels { get => UpperLeft.YInPixels + Distance.FromTiles(1).InPixels; }
    public int LeftInPixels { get => UpperLeft.XInPixels; }
    public int RightInPixels { get => UpperLeft.XInPixels + Distance.FromTiles(1).InPixels; }

    public bool IsCollidingWith(BodyBehavior body)
    {
        var collidingX = body.RightInPixels >= LeftInPixels && body.LeftInPixels <= RightInPixels;
        var collidingY = body.BottomInPixels >= TopInPixels && body.TopInPixels <= BottomInPixels;
        return collidingX && collidingY;
    }
    public void ShoveOut(BodyBehavior body)
    {
        if (body.Radius.InPixels * 2 > Distance.FromTiles(1).InPixels)
        {
            throw new Exception("large radius not supported");
        }

        var dx = LeftInPixels - body.LeftInPixels;
        var dy = TopInPixels - body.TopInPixels;
        if (Math.Abs(dx) > Math.Abs(dy)) // x's are more different, so shove out that way
        {
            if (dx > 0) // body is to the left
            {
                body.RightInPixels = LeftInPixels;
            }
            else
            {
                body.LeftInPixels = RightInPixels;
            }
        }
        else
        {
            if (dy > 0) // body is above
            {
                body.BottomInPixels = TopInPixels;
            }
            else
            {
                body.TopInPixels = BottomInPixels;
            }
        }
    }
}