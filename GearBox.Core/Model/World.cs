namespace GearBox.Core.Model;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Static;
using GearBox.Core.Utils;

/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World
{
    private readonly List<WorldTimer> _timers = [];
    private readonly LootTable _loot;
    
    // todo player-interactables
    private readonly SafeList<LootChest> _lootChests = new(); 
    private readonly SafeList<PlayerCharacter> _players = new();
    
    private readonly List<Func<Character>> _enemies = [];

    public World(
        Guid? id = null, 
        Map? map = null, 
        IItemTypeRepository? itemTypes = null, 
        LootTable? loot = null,
        List<Func<Character>>? enemies = null
    )
    {
        Id = id ?? Guid.NewGuid();
        Map = map ?? new();
        DynamicContent = new DynamicWorldContent();
        ItemTypes = itemTypes ?? ItemTypeRepository.Empty();
        _loot = loot ?? new LootTable();
        _enemies = enemies ?? [];

        if (_enemies.Count == 0)
        {
            _enemies.Add(() => new Character("Default enemy", 1));
        }
    }

    public Guid Id { get; init; }
    public Map Map { get; init; }
    public DynamicWorldContent DynamicContent { get; init; }
    public IItemTypeRepository ItemTypes { get; init; }


    /// <summary>
    /// Spawns a player into the world an heals them back to full,
    /// if they are not already in the world
    /// </summary>
    public void SpawnPlayer(PlayerCharacter player)
    {
        if (ContainsPlayer(player))
        {
            return;
        }
        player.HealPercent(100.0);
        DynamicContent.AddDynamicObject(player);
        _players.Add(player);
        player.Termination.Terminated += (sender, args) => RemovePlayer(player);
    }
    
    public bool ContainsPlayer(PlayerCharacter player)
    {
        return _players.Contains(player);
    }

    public void RemovePlayer(PlayerCharacter player)
    {
        if (!ContainsPlayer(player))
        {
            return;
        }
        DynamicContent.RemoveDynamicObject(player);
        _players.Remove(player);
    }
    
    public void AddTimer(WorldTimer timer)
    {
        _timers.Add(timer);
    }

    public void SpawnLootChest()
    {
        var inventory = _loot.GetRandomItems();
        var location = Map.GetRandomOpenTile();
        if (location != null)
        {
            var lootChest = new LootChest(location.Value.CenteredOnTile(), inventory);
            _lootChests.Add(lootChest);
            DynamicContent.AddDynamicObject(lootChest);
        }
    }

    public Character SpawnEnemy()
    {
        var enemyFactory = _enemies[Random.Shared.Next(_enemies.Count)];
        var enemy = enemyFactory.Invoke();
        
        var tile = Map.GetRandomOpenTile() ?? throw new Exception("Map has no open tiles");
        enemy.Coordinates = tile.CenteredOnTile();

        DynamicContent.AddDynamicObject(enemy);
        return enemy;
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    public void Update()
    {
        _timers.ForEach(t => t.Update());
        DynamicContent.Update();
        foreach (var obj in DynamicContent.DynamicObjects)
        {
            var body = obj.Body;
            if (body is not null)
            {
                Map.CheckForCollisions(body);
                DynamicContent.CheckForCollisions(body);
            }
        }
        foreach (var lootChest in _lootChests.AsEnumerable())
        {
            foreach (var player in _players.AsEnumerable())
            {
                lootChest.CheckForCollisions(player);
            }
        }
        _lootChests.ApplyChanges();
        _players.ApplyChanges();
    }

    public WorldInitJson GetWorldInitJsonFor(PlayerCharacter player)
    {
        var result = new WorldInitJson(
            player.Id,
            Map.ToJson(),
            ItemTypes.ToJson()
        );
        return result;
    }

    /// <summary>
    /// Gets the JSON for a world update, 
    /// except it includes all stable values, 
    /// regardless of whether they've changed
    /// </summary>
    public WorldUpdateJson GetCompleteWorldUpdateJson()
    {
        var result = new WorldUpdateJson(DynamicContent.ToJson(true));
        return result;
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