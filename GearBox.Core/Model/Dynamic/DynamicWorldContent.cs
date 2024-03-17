using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// Dynamic content can change with each update.
/// </summary>
public class DynamicWorldContent : ISerializable<DynamicWorldContentJson>
{
    private readonly SafeList<IDynamicGameObject> _gameObjects = new();

    public IEnumerable<IDynamicGameObject> DynamicObjects => _gameObjects.AsEnumerable();

    public void AddDynamicObject(IDynamicGameObject obj)
    {
        if (!_gameObjects.Contains(obj))
        {
            _gameObjects.Add(obj);
        }
    }

    public void RemoveDynamicObject(IDynamicGameObject obj)
    {
        _gameObjects.Remove(obj);
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    public void Update()
    {
        _gameObjects.ApplyChanges();
        foreach (var item in _gameObjects.AsEnumerable())
        {
            item.Update();
        }
        _gameObjects.ApplyChanges();
    }

    public DynamicWorldContentJson ToJson()
    {
        var objs = _gameObjects.AsEnumerable()
            .Select(obj => obj.ToJson())
            .ToList();
        return new DynamicWorldContentJson(objs);
    }
}