using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects;

public class TerminateBehavior
{
    private readonly IGameObject _obj;
    private readonly Func<bool> _isTerminated;

    public TerminateBehavior(IGameObject obj, Func<bool> isTerminated)
    {
        _obj = obj;
        _isTerminated = isTerminated;
    }

    public bool IsTerminated => _isTerminated.Invoke();
    public EventEmitter<TerminateEvent> EventTerminated { get; } = new();

    public void OnTerminated()
    {
        EventTerminated.ProcessEvent(new TerminateEvent(_obj), _ => {});
    }
}