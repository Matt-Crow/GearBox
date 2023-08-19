namespace GearBox.Core.Model.Units;

public class Speed
{
    private double _pixelsPerFrame;


    private Speed(double pixelsPerFrame)
    {
        _pixelsPerFrame = pixelsPerFrame;
    }

    public static Speed InTilesPerSecond(double tilesPerSecond)
    {
        /*
            tile     pixel   second
            ------ x ----- x ------
            second   tile    frame
        */
        var result = tilesPerSecond * Tile.SIZE / Time.FRAMES_PER_SECOND;
        return new Speed(result);
    }


    public double InPixelsPerFrame { get => _pixelsPerFrame; }
}