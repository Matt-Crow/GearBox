using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public class ChangeTracker<TJson>
{
    private readonly IDynamic<TJson> _target;
    private readonly Serializer _serializer;
    private bool _hasChangedSinceLastTick = true;
    private int _lastHash;

    public ChangeTracker(IDynamic<TJson> target)
    {
        _target = target;
        _serializer = new Serializer("", _target.Serialize);
        _lastHash = CurrentHash();
    }

    public bool HasChanged => _lastHash != CurrentHash();

    public void Update()
    {
        _hasChangedSinceLastTick = HasChanged;
        _lastHash = CurrentHash();
    }

    private int CurrentHash()
    {
        var result = new HashCode();
        foreach (var field in _target.DynamicValues)
        {
            result.Add(field);
        }
        var hashCode = result.ToHashCode();
        return hashCode;
    }

    public MaybeChangeJson<TJson> ToJson()
    {
        var result = _hasChangedSinceLastTick
            ? MaybeChangeJson<TJson>.Changed(_target.AsJson())
            : MaybeChangeJson<TJson>.NoChanges();
        return result;
    }
}