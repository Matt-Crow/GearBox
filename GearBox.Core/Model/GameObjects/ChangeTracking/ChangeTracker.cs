using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public class ChangeTracker
{
    private readonly IDynamic _target;
    private readonly Serializer _serializer;
    private bool _hasChangedSinceLastTick = true;
    private int _lastHash;

    public ChangeTracker(IDynamic target)
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

    public StableJson ToJson()
    {
        var result = _hasChangedSinceLastTick
            ? StableJson.Changed(_serializer.Serialize().Content)
            : StableJson.NoChanges();
        return result;
    }
}