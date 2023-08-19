namespace GearBox.Core.Model.Units;

public readonly struct Speed
{
    private readonly double _pixelsPerFrame;


    private Speed(double pixelsPerFrame)
    {
        _pixelsPerFrame = pixelsPerFrame;
    }

    public static Speed InTilesPerSecond(int tilesPerSecond)
    {
        /*
            tile     pixel   second
            ------ x ----- x ------
            second   tile    frame
        */
        var pixelsPerSecond = Distance.FromTiles(tilesPerSecond).InPixels;
        var result = pixelsPerSecond / Time.FRAMES_PER_SECOND;
        return new Speed(result);
    }


    public double InPixelsPerFrame { get => _pixelsPerFrame; }
}