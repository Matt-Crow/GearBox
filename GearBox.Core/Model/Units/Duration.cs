namespace GearBox.Core.Model.Units;

public readonly struct Duration
{
    private static readonly int FPS = 20;
    private readonly int _frames;

    private Duration(int frames)
    {
        _frames = frames;
    }

    public static Duration FromFrames(int frames) => new(frames);
    public static Duration FromSeconds(double seconds) => FromFrames((int)(FPS * seconds));

    public int InFrames => _frames;
    public double InSeconds => ((double)_frames) / FPS;
}