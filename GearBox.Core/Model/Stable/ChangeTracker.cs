namespace GearBox.Core.Model.Stable;

public class ChangeTracker
{
    private int? _lastHash = null;
    private readonly DynamicHasher _hasher = new();
    private readonly IDynamic _target;

    public ChangeTracker(IDynamic target)
    {
        _target = target;
    }

    public bool HasChanged => _lastHash != _hasher.Hash(_target);

    public void Update()
    {
        _lastHash = _hasher.Hash(_target);
    }
}