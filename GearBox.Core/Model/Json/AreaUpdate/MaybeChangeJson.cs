namespace GearBox.Core.Model.Json.AreaUpdate;

public readonly struct MaybeChangeJson<T>
{
    private MaybeChangeJson(T? value, bool hasChanged)
    {
        Value = value;
        HasChanged = hasChanged;
    }

    public static MaybeChangeJson<T> Changed(T value) => new(value, true);
    public static MaybeChangeJson<T> NoChanges() => new(default, false);

    public T? Value { get; init; }
    public bool HasChanged { get; init; }
}