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
        DynamicContent = new DynamicWorldContent();
    }


    public Guid Id { get; init; }
    public StaticWorldContent StaticContent { get; init; }
    public DynamicWorldContent DynamicContent { get; init; }
    public IEnumerable<IDynamicGameObject> DynamicObjects { get => DynamicContent.DynamicObjects; }


    public void AddDynamicObject(IDynamicGameObject obj)
    {
        DynamicContent.AddDynamicObject(obj);
    }

    public void RemoveDynamicObject(IDynamicGameObject obj)
    {
        DynamicContent.RemoveDynamicObject(obj);
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    public void Update()
    {
        DynamicContent.Update();
        foreach (var obj in DynamicObjects)
        {
            var body = obj.Body;
            if (body is not null)
            {
                StaticContent.CheckForCollisions(body);
            }
        }
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