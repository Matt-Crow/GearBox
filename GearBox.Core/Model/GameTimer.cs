namespace GearBox.Core.Model;

public class GameTimer
{
    private readonly Action _doThis;
    private readonly int _period;
    private int _ticks = 0;

    public GameTimer(Action doThis, int period)
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