namespace GearBox.Core.Model.GameObjects.ChangeTracking;

public class ChangeTracker
{
    private readonly IDynamic _target;
    private int _lastHash;

    public ChangeTracker(IDynamic target)
    {
        _target = target;
        _lastHash = CurrentHash();
    }

    public bool HasChanged => _lastHash != CurrentHash();

    public void Update()
    {
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
}