namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// Dynamic content can change with each update.
/// </summary>
public class DynamicWorldContent : ISerializable<DynamicWorldContentJson>
{
    /*
        List is probably more efficient for iteration,
        whereas Dictionary is more efficient for searching.
        I can change this implementation based on which of those two operations
        are needed more frequently... or I can do a more complex data structure.
    */
    private readonly List<IDynamicGameObject> _dynamicObjects = new();

    public IEnumerable<IDynamicGameObject> DynamicObjects { get => _dynamicObjects.AsEnumerable(); }

    public void AddDynamicObject(IDynamicGameObject obj)
    {
        EnsureNotYetAdded(obj);
        _dynamicObjects.Add(obj);
    }

    private void EnsureNotYetAdded(IGameObject obj)
    {
        if (_dynamicObjects.Contains(obj))
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    public void Update()
    {
        _dynamicObjects.ForEach(x => x.Update());
    }

    public DynamicWorldContentJson ToJson()
    {
        return new DynamicWorldContentJson();
    }
}