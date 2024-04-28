namespace GearBox.Core.Model.Stable;

public class ChangeTracker2
{
    private int? _lastHash = null;
    private readonly DynamicHasher _hasher = new();
    private readonly IDynamic _target;

    public ChangeTracker2(IDynamic target)
    {
        _target = target;
    }

    public bool HasChanged => _lastHash != _hasher.Hash(_target);

    public void Update()
    {
        _lastHash = _hasher.Hash(_target);
    }
}