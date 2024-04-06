namespace GearBox.Core.Model.Units;

public readonly struct Speed
{
    private readonly double _pixelsPerFrame;


    private Speed(double pixelsPerFrame)
    {
        _pixelsPerFrame = pixelsPerFrame;
    }

    public static Speed FromTilesPerSecond(int tilesPerSecond)
    {
        /*
            tile     pixel   second
            ------ x ----- x ------
            second   tile    frame
        */
        var pixels = Distance.FromTiles(tilesPerSecond).InPixels;
        var second = Duration.FromSeconds(1).InFrames;
        var result = pixels / second;
        return new Speed(result);
    }

    public static Speed FromPixelsPerFrame(double pixelsPerFrame) => new(pixelsPerFrame);


    public double InPixelsPerFrame => _pixelsPerFrame;
}