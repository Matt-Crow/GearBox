namespace GearBox.Core.Model;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
using System;

/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World
{
    private readonly List<WorldTimer> _timers = new();
    private readonly LootTable _loot;

    public World() : this(Guid.NewGuid(), StaticWorldContent.EMPTY)
    {

    }

    public World(Guid id) : this(id, StaticWorldContent.EMPTY)
    {
        
    }

    public World(Guid id, StaticWorldContent staticContent) : this(id, staticContent, ItemTypeRepository.Of(new List<ItemType>()), new LootTable())
    {

    }

    public World(Guid id, StaticWorldContent staticContent, IItemTypeRepository itemTypes, LootTable loot)
    {
        Id = id;
        StaticContent = staticContent;
        DynamicContent = new DynamicWorldContent();
        StableContent = new StableWorldContent();
        ItemTypes = itemTypes;
        _loot = loot;
    }

    public Guid Id { get; init; }
    public StaticWorldContent StaticContent { get; init; }
    public DynamicWorldContent DynamicContent { get; init; }
    public StableWorldContent StableContent { get; init; }
    public IItemTypeRepository ItemTypes { get; init; }
    public IEnumerable<IDynamicGameObject> DynamicObjects { get => DynamicContent.DynamicObjects; }


    public void AddDynamicObject(IDynamicGameObject obj)
    {
        DynamicContent.AddDynamicObject(obj);
    }

    public void RemoveDynamicObject(IDynamicGameObject obj)
    {
        DynamicContent.RemoveDynamicObject(obj);
    }

    public void AddTimer(WorldTimer timer)
    {
        _timers.Add(timer);
    }

    public void SpawnLootChest()
    {
        var random = new Random();

        var chestItems = new List<IItem>();
        var numItems = random.Next(1, 4);
        for (var i = 0; i < numItems; i++)
        {
            chestItems.Add(_loot.GetRandomItem());
        }
        
        var location = StaticContent.Map.GetRandomOpenTile();
        if (location != null)
        {
            var lootChest = new LootChest(location.Value, chestItems.ToArray());
            StableContent.AddLootChest(lootChest);
        }
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    /// <returns>Changes to stable content</returns>
    public IEnumerable<Change> Update()
    {
        _timers.ForEach(t => t.Update());
        DynamicContent.Update();
        foreach (var obj in DynamicObjects)
        {
            var body = obj.Body;
            if (body is not null)
            {
                StaticContent.CheckForCollisions(body);
            }
        }
        return StableContent.Update();
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