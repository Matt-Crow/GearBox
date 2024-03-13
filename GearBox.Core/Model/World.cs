namespace GearBox.Core.Model;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Static;

/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World
{
    private readonly List<WorldTimer> _timers = new();
    private readonly LootTable _loot;

    public World(
        Guid? id = null, 
        StaticWorldContent? staticContent = null, 
        IItemTypeRepository? itemTypes = null, 
        LootTable? loot = null
    )
    {
        Id = id ?? Guid.NewGuid();
        StaticContent = staticContent ?? StaticWorldContent.EMPTY;
        DynamicContent = new DynamicWorldContent();
        StableContent = new StableWorldContent();
        ItemTypes = itemTypes ?? ItemTypeRepository.Empty();
        _loot = loot ?? new LootTable();
    }

    public Guid Id { get; init; }
    public StaticWorldContent StaticContent { get; init; }
    public DynamicWorldContent DynamicContent { get; init; }
    public StableWorldContent StableContent { get; init; }
    public IItemTypeRepository ItemTypes { get; init; }

    public void AddTimer(WorldTimer timer)
    {
        _timers.Add(timer);
    }

    public void SpawnLootChest()
    {
        var chestItems = new List<IItem>();
        var numItems = Random.Shared.Next(0, 3) + 1;
        for (var i = 0; i < numItems; i++)
        {
            chestItems.Add(_loot.GetRandomItem());
        }
        
        var location = StaticContent.Map.GetRandomOpenTile();
        if (location != null)
        {
            var lootChest = new LootChest(location.Value.CenteredOnTile(), chestItems.ToArray());
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
        foreach (var obj in DynamicContent.DynamicObjects)
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