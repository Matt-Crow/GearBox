namespace GearBox.Core.Model.Stable;

public class StableWorldContent
{
    private readonly List<IStableGameObject> _objects = new();
    private readonly Dictionary<IStableGameObject, int> _hashes = new();
    private readonly List<Change> _pendingChanges = new();

    public void Add(IStableGameObject obj)
    {
        _objects.Add(obj);
        _hashes[obj] = MakeHashFor(obj);
        _pendingChanges.Add(Change.Created(obj));
    }

    private static int MakeHashFor(IStableGameObject obj)
    {
        var result = new HashCode();
        foreach (var field in obj.DynamicValues)
        {
            result.Add(field);
        }
        return result.ToHashCode();
    }

    public void ClearPendingChanges()
    {
        _pendingChanges.Clear();
    }

    // will be used during WorldInit
    public IEnumerable<Change> GetPendingChanges()
    {
        return _pendingChanges.AsEnumerable();
    }

    /// <summary>
    /// Updates all dynamic objects
    /// </summary>
    /// <returns>all the changes which have occured since the last update</returns>
    public IEnumerable<Change> Update()
    {
        var result = new List<Change>();
        foreach (var obj in _objects)
        {
            obj.Update();
        }
        foreach (var obj in _objects.Where(HashHasChanged))
        {
            result.Add(Change.Updated(obj));
            _hashes[obj] = MakeHashFor(obj);
        }
        result.AddRange(_pendingChanges); // find any changes which occured during update
        ClearPendingChanges();
        return result;
    }

    private bool HashHasChanged(IStableGameObject obj)
    {
        return _hashes[obj] != MakeHashFor(obj);
    }
}