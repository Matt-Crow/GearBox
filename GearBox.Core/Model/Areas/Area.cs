using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Ai;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Json.AreaInit;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Static;
using GearBox.Core.Utils;
using GearBox.Core.Model.Json.AreaUpdate;
using GearBox.Core.Model.Units;
using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Areas;

public class Area : IArea
{
    private readonly IGame _game;
    private readonly GameObjectCollection<Character> _characters = new();
    private readonly GameObjectCollection<Projectile> _projectiles = new();
    private readonly GameObjectCollection<LootChest> _lootChests = new(); 
    private readonly SafeList<PlayerCharacter> _players = new();
    private readonly List<GameTimer> _timers = [];
    private readonly Team _playerTeam = new("Players");
    private readonly Team _enemyTeam = new("Enemies");
    private readonly Map _map;
    private readonly LootTable _loot;
    private readonly List<Func<Character>> _enemyMakers;

    public Area(
        string? name = null,
        IGame? game = null,
        Map? map = null, 
        LootTable? loot = null,
        List<Func<Character>>? enemyMakers = null
    )
    {
        Name = name ?? "an area";
        _game = game ?? new Game();
        _map = map ?? new();
        _loot = loot ?? new LootTable();
        _enemyMakers = enemyMakers ?? [];

        if (_enemyMakers.Count == 0)
        {
            _enemyMakers.Add(() => new Character("Default enemy", 1));
        }
    }

    public string Name { get; init; }

    public void SpawnPlayer(PlayerCharacter player)
    {
        if (_players.Contains(player))
        {
            return;
        }
        player.HealPercent(100.0);
        player.SetArea(this);
        player.Team = _playerTeam;
        _characters.AddGameObject(player);
        _players.Add(player);
        player.Termination.Terminated += (sender, args) => RemovePlayer(player);
    }

    public Character SpawnEnemy()
    {
        var enemyFactory = _enemyMakers[Random.Shared.Next(_enemyMakers.Count)];
        var enemy = enemyFactory.Invoke();
        enemy.AiBehavior = new WanderAiBehavior(enemy);
        enemy.SetArea(this);
        enemy.Team = _enemyTeam;
        
        var tile = _map.GetRandomFloorTile();
        enemy.Coordinates = tile.CenteredOnTile();

        _characters.AddGameObject(enemy);
        return enemy;
    }

    public void SpawnLootChest()
    {
        var inventory = _loot.GetRandomItems();
        var location = _map.GetRandomFloorTile();
        var lootChest = new LootChest(location.CenteredOnTile(), inventory);
        _lootChests.AddGameObject(lootChest);
    }
    
    public void AddProjectile(Projectile projectile) => _projectiles.AddGameObject(projectile);
    public void AddTimer(GameTimer timer) => _timers.Add(timer);

    public void RemovePlayer(PlayerCharacter player)
    {
        if (!_players.Contains(player))
        {
            return;
        }

        player.SetArea(null); // might change when moving player to new area
        _characters.RemoveGameObject(player);
        _players.Remove(player);
    }

    /// <summary>
    /// might be able to remove this once commands operate on the game instead of area
    /// </summary>
    public CraftingRecipe? GetCraftingRecipeById(Guid id) => _game.GetCraftingRecipeById(id);

    public Character? GetNearestEnemy(Character character)
    {
        var result = _characters.AsEnumerable
            .Where(other => other.Team != character.Team)
            .OrderBy(enemy => enemy.Coordinates.DistanceFrom(character.Coordinates).InPixels)
            .FirstOrDefault();
        return result;
    }

    public Coordinates GetRandomFloorTile() => _map.GetRandomFloorTile();

    public MapJson GetMapJson() => _map.ToJson();

    public AreaUpdateJson GetAreaUpdateJsonFor(PlayerCharacter player)
    {
        var allGameObjects = Array.Empty<GameObjectJson>()
            .Concat(_characters.ToJson())
            .Concat(_projectiles.ToJson())
            .Concat(_lootChests.ToJson())
            .ToList();
        return new(allGameObjects, player.GetChanges());
    }

    public void Update()
    {
        _timers.ForEach(t => t.Update());
        _characters.Update();
        _projectiles.Update();
        _lootChests.Update();
        foreach (var character in _characters.AsEnumerable)
        {
            _map.CheckForCollisions(character.Body);
        }
        foreach (var projectile in _projectiles.AsEnumerable)
        {
            _map.CheckForCollisions(projectile.Body);
            CheckForCollisionsWithCharacters(projectile.Body);
        }
        foreach (var lootChest in _lootChests.AsEnumerable)
        {
            foreach (var player in _players.AsEnumerable())
            {
                lootChest.CheckForCollisions(player);
            }
        }
        _players.ApplyChanges();
    }

    private void CheckForCollisionsWithCharacters(BodyBehavior body)
    {
        var collidingCharacters = _characters.AsEnumerable
            .Where(obj => obj.Body.CollidesWith(body));
        foreach (var character in collidingCharacters)
        {
            body.OnCollided(new CollideEventArgs(character));
        }
    }
}