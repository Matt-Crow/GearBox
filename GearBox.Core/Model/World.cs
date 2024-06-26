using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Ai;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Static;
using GearBox.Core.Utils;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model;
/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World : IArea
{
    private readonly List<GameTimer> _timers = [];
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

    #region Implement IArea
    public void SpawnPlayer(PlayerCharacter player)
    {
        if (ContainsPlayer(player))
        {
            return;
        }
        player.HealPercent(100.0);
        player.SetArea(this);
        player.Team = _playerTeam;
        GameObjects.AddGameObject(player);
        _players.Add(player);
        player.Termination.Terminated += (sender, args) => RemovePlayer(player);
    }

    public Character SpawnEnemy()
    {
        var enemyFactory = _enemies[Random.Shared.Next(_enemies.Count)];
        var enemy = enemyFactory.Invoke();
        enemy.AiBehavior = new WanderAiBehavior(enemy);
        enemy.SetArea(this);
        enemy.Team = _enemyTeam;
        
        var tile = Map.FindRandomFloorTile() ?? throw new Exception("Map has no open tiles");
        enemy.Coordinates = tile.CenteredOnTile();

        GameObjects.AddGameObject(enemy);
        return enemy;
    }
    
    public void AddProjectile(Projectile projectile) => GameObjects.AddGameObject(projectile);
    public CraftingRecipe? GetCraftingRecipeById(Guid id) => CraftingRecipes.GetById(id);

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
    #endregion
    
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
    
    public void AddTimer(GameTimer timer)
    {
        _timers.Add(timer);
    }

    public void SpawnLootChest()
    {
        var inventory = _loot.GetRandomItems();
        var location = Map.FindRandomFloorTile();
        if (location != null)
        {
            var lootChest = new LootChest(location.Value.CenteredOnTile(), inventory);
            _lootChests.Add(lootChest);
            GameObjects.AddGameObject(lootChest);
        }
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
            // todo move Projectiles to their own list
            if (obj is Projectile projectile)
            {
                Map.CheckForCollisions(projectile);
            } else if (obj.Body != null)
            {
                Map.CheckForCollisions(obj.Body);
            }

            var body = obj.Body;
            if (body is not null)
            {
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

    public AreaUpdateJson GetWorldUpdateJsonFor(PlayerCharacter player)
    {
        var result = new AreaUpdateJson(
            GameObjects.ToJson(),
            player.GetChanges()
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