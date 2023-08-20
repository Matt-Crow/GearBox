namespace GearBox.Core.Model;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Static;
using System;

/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World
{
    /*
        List is probably more efficient for iteration,
        whereas Dictionary is more efficient for searching.
        I can change this implementation based on which of those two operations
        are needed more frequently... or I can do a more complex data structure.
    */
    private readonly List<IDynamicGameObject> _dynamicObjects = new();


    public World() : this(Guid.NewGuid(), StaticWorldContent.EMPTY)
    {

    }

    public World(Guid id) : this(id, StaticWorldContent.EMPTY)
    {
        
    }

    public World(Guid id, StaticWorldContent staticContent)
    {
        Id = id;
        StaticContent = staticContent;
    }


    public Guid Id { get; init; }
    public StaticWorldContent StaticContent { get; init; }
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

    public override bool Equals(object? other)
    {
        var otherWorld = other as World;
        return Id.Equals(otherWorld?.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}