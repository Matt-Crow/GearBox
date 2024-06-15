using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Ai;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Static;
using GearBox.Core.Utils;

namespace GearBox.Core.Model;
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
    private readonly Team _playerTeam = new("Players");
    private readonly Team _enemyTeam = new("Enemies");

    public World(
        Guid? id = null, 
        Map? map = null, 
        IItemTypeRepository? itemTypes = null, 
        CraftingRecipeRepository? craftingRecipes = null,
        LootTable? loot = null,
        List<Func<Character>>? enemies = null
    )
    {
        Id = id ?? Guid.NewGuid();
        Map = map ?? new();
        GameObjects = new GameObjectCollection();
        ItemTypes = itemTypes ?? ItemTypeRepository.Empty();
        CraftingRecipes = craftingRecipes ?? CraftingRecipeRepository.Empty();
        _loot = loot ?? new LootTable();
        _enemies = enemies ?? [];

        if (_enemies.Count == 0)
        {
            _enemies.Add(() => new Character("Default enemy", 1));
        }
    }

    public Guid Id { get; init; }
    public Map Map { get; init; }
    public GameObjectCollection GameObjects { get; init; }
    public IItemTypeRepository ItemTypes { get; init; }
    public CraftingRecipeRepository CraftingRecipes { get; init; }

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
        player.World = this;
        player.Team = _playerTeam;
        GameObjects.AddGameObject(player);
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
        GameObjects.RemoveGameObject(player);
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
            GameObjects.AddGameObject(lootChest);
        }
    }

    public Character SpawnEnemy()
    {
        var enemyFactory = _enemies[Random.Shared.Next(_enemies.Count)];
        var enemy = enemyFactory.Invoke();
        enemy.AiBehavior = new WanderAiBehavior(enemy);
        enemy.World = this;
        enemy.Team = _enemyTeam;
        
        var tile = Map.GetRandomOpenTile() ?? throw new Exception("Map has no open tiles");
        enemy.Coordinates = tile.CenteredOnTile();

        GameObjects.AddGameObject(enemy);
        return enemy;
    }

    public Character? GetNearestEnemy(Character character)
    {
        var result = GameObjects.GameObjects
            .Select(obj => obj as Character)
            .Where(maybeCharacter => maybeCharacter != null)
            .Select(character => character!)
            .Where(other => other != character) // not the same
            .Where(other => other.Team != character.Team)
            .OrderBy(enemy => enemy.Coordinates.DistanceFrom(character.Coordinates).InPixels)
            .FirstOrDefault();
        return result;
    }

    /// <summary>
    /// Called each game tick.
    /// Updates the world and everything in it
    /// </summary>
    public void Update()
    {
        _timers.ForEach(t => t.Update());
        GameObjects.Update();
        foreach (var obj in GameObjects.GameObjects)
        {
            var body = obj.Body;
            if (body is not null)
            {
                Map.CheckForCollisions(body);
                GameObjects.CheckForCollisions(body);
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
            ItemTypes.ToJson(),
            CraftingRecipes.ToJson()
        );
        return result;
    }

    public WorldUpdateJson GetWorldUpdateJsonFor(PlayerCharacter player)
    {
        var result = new WorldUpdateJson(
            GameObjects.ToJson(), 
            player.Inventory.ToJson(), 
            player.WeaponSlot.ToJson(),
            player.ArmorSlot.ToJson(),
            player.GetStatSummaryJson()
        );
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