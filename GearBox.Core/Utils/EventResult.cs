namespace GearBox.Core.Utils;

public class EventResult<T>
{
    private EventResult(T? value, bool isCanceled)
    {
        Value = value;
        IsCanceled = isCanceled;
    }

    public static EventResult<T> ValueResult(T value) => new(value, false);
    public static EventResult<T> CanceledResult() => new(default, true);

    public T? Value { get; init; }
    public bool IsCanceled { get; init; }
}