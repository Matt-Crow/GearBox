using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Dynamic;

/// <summary>
/// Dynamic content can change with each update.
/// </summary>
public class DynamicWorldContent
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

    public void CheckForCollisions(BodyBehavior body)
    {
        // only check for collisions with Characters for now
        var collidingCharacters = _gameObjects.AsEnumerable()
            .Select(obj => obj as Character)
            .Where(obj => obj != null)
            .Select(obj => obj!)
            .Where(obj => obj?.Body != null && obj.Body.CollidesWith(body) && obj.Body != body);
        foreach (var character in collidingCharacters)
        {
            body.OnCollided(new CollideEventArgs(character));
        }
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
        foreach (var item in _gameObjects.AsEnumerable().Where(IsTerminated))
        {
            item.Termination?.OnTerminated();
            _gameObjects.Remove(item);
        }
        _gameObjects.ApplyChanges();
    }

    private static bool IsTerminated(IDynamicGameObject obj)
    {
        return obj.Termination != null && obj.Termination.IsTerminated;
    }

    public List<GameObjectJson> ToJson(bool isWorldInit)
    {
        var objs = _gameObjects.AsEnumerable()
            .Where(obj => obj.Serializer is not null)
            .Select(obj => obj.Serializer!.Serialize(isWorldInit))
            .ToList();
        return objs;
    }
}