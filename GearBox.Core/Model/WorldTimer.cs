namespace GearBox.Core.Model;

public class WorldTimer
{
    private readonly Action _doThis;
    private readonly int _period;
    private int _ticks = 0;

    public WorldTimer(Action doThis, int period)
    {
        _doThis = doThis;
        _period = period;
    }

    public void Update()
    {
        _ticks += 1;
        if (_ticks == _period)
        {
            _doThis();
            _ticks = 0;
        }
    }
}