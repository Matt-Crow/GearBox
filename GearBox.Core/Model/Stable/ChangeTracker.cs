namespace GearBox.Core.Model.Stable;

/// <summary>
/// Monitors stable game objects to see if they change
/// </summary>
public class ChangeTracker
{
    private readonly DynamicHasher _hasher = new();
    private readonly IStableGameObject _gameObject;
    private int? _lastHash = null;

    public ChangeTracker(IStableGameObject gameObject)
    {
        _gameObject = gameObject;
        State = Change.Content(gameObject);
    }

    public bool HasChanged =>  _lastHash != _hasher.Hash(_gameObject);
    public Change State { get; private set; }

    public void OnRemoved()
    {
        State = Change.Removed(_gameObject);
        _lastHash = null; // force a state has changed
    }

    public void OnUpdateEnd()
    {
        if (!State.IsDelete)
        {
            _lastHash = _hasher.Hash(_gameObject);
        }
    }
}