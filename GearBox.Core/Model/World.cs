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

    public World() : this(Guid.NewGuid(), StaticWorldContent.EMPTY)
    {

    }

    public World(Guid id) : this(id, StaticWorldContent.EMPTY)
    {
        
    }

    public World(Guid id, StaticWorldContent staticContent) : this(id, staticContent, ItemTypeRepository.Of(new List<ItemType>()))
    {

    }

    public World(Guid id, StaticWorldContent staticContent, IItemTypeRepository itemTypes)
    {
        Id = id;
        StaticContent = staticContent;
        DynamicContent = new DynamicWorldContent();
        StableContent = new StableWorldContent();
        ItemTypes = itemTypes;
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

        var chestItems = new List<Item>();
        var itemTypes = ItemTypes.GetAll().ToList();
        var numItems = random.Next(1, 4);
        for (var i = 0; i < numItems; i++)
        {
            var itemType = itemTypes[random.Next(itemTypes.Count)];
            chestItems.Add(new Item(itemType));
        }
        // todo Map.GetOpenSpot
        var location = Coordinates.FromTiles(
            random.Next(StaticContent.Map.Width.InTiles), 
            random.Next(StaticContent.Map.Height.InTiles)
        );
        var lootChest = new LootChest(location, chestItems.ToArray());
        StableContent.AddLootChest(lootChest);
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