using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// GameObjects in an area
/// </summary>
public class GameObjectCollection<T>
where T : IGameObject
{
    private readonly SafeList<T> _gameObjects = new();

    public IEnumerable<T> AsEnumerable => _gameObjects.AsEnumerable();

    public void AddGameObject(T obj)
    {
        if (!_gameObjects.Contains(obj))
        {
            _gameObjects.Add(obj);
        }
    }

    public void RemoveGameObject(T obj)
    {
        _gameObjects.Remove(obj);
    }

    public void Update()
    {
        _gameObjects.ApplyChanges();
        foreach (var item in _gameObjects.AsEnumerable())
        {
            item.Update();
        }
        foreach (var item in _gameObjects.AsEnumerable().Where(IsTerminated))
        {
            item.Termination?.OnTerminated();
            _gameObjects.Remove(item);
        }
        _gameObjects.ApplyChanges();
    }

    private static bool IsTerminated(T obj)
    {
        return obj.Termination != null && obj.Termination.IsTerminated;
    }

    public List<GameObjectJson> ToJson()
    {
        var objs = _gameObjects.AsEnumerable()
            .Where(obj => obj.Serializer is not null)
            .Select(obj => obj.Serializer!.Serialize())
            .ToList();
        return objs;
    }
}