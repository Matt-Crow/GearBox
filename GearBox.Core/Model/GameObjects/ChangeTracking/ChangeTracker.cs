using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public class ChangeTracker<TJson>
{
    private readonly IMightChange<TJson> _target;
    private bool _hasChangedSinceLastTick = true;
    private int _lastHash;

    public ChangeTracker(IMightChange<TJson> target)
    {
        _target = target;
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
            ? MaybeChangeJson<TJson>.Changed(_target.ToJson())
            : MaybeChangeJson<TJson>.NoChanges();
        return result;
    }
}