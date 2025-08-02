namespace GearBox.Core.Utils;

/// <summary>
/// Emits events so other classes can react to them.
/// This is more flexible than C#'s built-in events.
/// </summary>
public class EventEmitter<T>
{
    private readonly TerminableList<Func<T, T>> _modifiers = new();
    private readonly TerminableList<Func<T, bool>> _cancelers = new();
    private readonly TerminableList<Action<T>> _listeners = new();

    /// <summary>
    /// The modifier takes the event, then returns the modified event.
    /// It is OK if it modifies the event.
    /// </summary>
    public void AddModifier(Func<T, T> modifier, int? times = null)
    {
        _modifiers.Add(modifier, times);
    }

    /// <summary>
    /// If the canceler returns true, cancels the event.
    /// </summary>
    public void AddCanceler(Func<T, bool> canceler, int? times = null)
    {
        _cancelers.Add(canceler, times);
    }

    /// <summary>
    /// The listener is invoked when this emits an event.
    /// </summary>
    public void AddListener(Action<T> listener, int? times = null)
    {
        _listeners.Add(listener, times);
    }

    /// <summary>
    /// Notifies listeners that the given event occurred.
    /// </summary>
    public EventResult<T> EmitEvent(T e)
    {
        _modifiers.Iterate(modifier => e = modifier(e));

        var cancelIt = false;
        _cancelers.Iterate(canceler =>
        {
            if (canceler(e))
            {
                cancelIt = true;
            }
        });
        if (cancelIt)
        {
            return EventResult<T>.CanceledResult();
        }

        _listeners.Iterate(listener => listener(e));

        return EventResult<T>.ValueResult(e);
    }

    private class TerminableList<U>
    {
        private readonly List<RunsUpToXTimes<U>> _inner = [];

        public void Add(U value, int? times = null)
        {
            _inner.Add(new RunsUpToXTimes<U>(value, times));
        }

        public void Iterate(Action<U> visitor)
        {
            foreach (var item in _inner)
            {
                item.Tick();
                visitor(item.Inner);
            }
            _inner.RemoveAll(item => item.IsDone);
        }
    }

    private class RunsUpToXTimes<U>
    {
        private int? _times;

        public RunsUpToXTimes(U inner, int? times)
        {
            Inner = inner;
            _times = times;
        }

        public U Inner { get; init; }

        public bool IsDone => _times <= 0;

        public void Tick() => _times--;
    }
}