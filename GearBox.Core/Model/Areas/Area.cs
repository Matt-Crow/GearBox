using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Utils;
using GearBox.Core.Model.Json.AreaUpdate;
using GearBox.Core.Model.Units;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.Json.AreaUpdate.GameObjects;

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
    private readonly IEnemyFactory _enemyFactory;
    private readonly List<IExit> _exits = [];

    public Area(
        string? name = null,
        int level = 1,
        IGame? game = null,
        Map? map = null, 
        List<ItemShop>? shops = null,
        LootTable? loot = null,
        IEnemyFactory? enemyFactory = null,
        List<IExit>? exits = null 
    )
    {
        Name = name ?? "an area";
        Level = level;
        _game = game ?? new Game();
        _map = map ?? new();
        Shops = shops ?? [];
        _loot = loot ?? new LootTable([], new RandomNumberGenerator());
        _enemyFactory = enemyFactory ?? EnemyFactory.MakeDefault();
        _exits = exits ?? [];

        AddTimer(_enemyFactory.MakeSpawnTimer(this));
    }

    public string Name { get; init; }
    public int Level { get; init; }
    public Dimensions Bounds => _map.Bounds;
    public List<ItemShop> Shops { get; private init; }

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
        player.Termination.EventTerminated.AddListener(_ => RemovePlayer(player));
    }

    public EnemyCharacter? SpawnEnemy()
    {
        var enemy = _enemyFactory.MakeRandom(Level);
        if (enemy == null)
        {
            return null;
        }
        
        enemy.SetArea(this);
        enemy.Team = _enemyTeam;
        
        var tile = _map.GetRandomFloorTile();
        enemy.Coordinates = tile.CenteredOnTile();

        _characters.AddGameObject(enemy);
        return enemy;
    }

    public LootChest? SpawnLootChest()
    {
        var inventory = _loot.GetRandomLoot(); 
        if (inventory.IsEmpty)
        {
            return null; // don't bother spawning chest with no loot
        }

        var location = _map.GetRandomFloorTile();
        var lootChest = new LootChest(location.CenteredOnTile(), inventory);
        _lootChests.AddGameObject(lootChest);
        return lootChest;
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

    public PlayerCharacter? GetNearestPlayerTo(EnemyCharacter enemy)
    {
        var result = _players.AsEnumerable()
            .OrderBy(p => p.Coordinates.DistanceFrom(enemy.Coordinates).InPixels)
            .FirstOrDefault();
        return result;
    }

    public Coordinates GetRandomFloorTile() => _map.GetRandomFloorTile();

    public AreaJson ToJson()
    {
        var result = new AreaJson(
            Name, 
            _map.ToJson(), 
            Shops
                .Select(s => s.ToJson())
                .ToList()
        );
        return result;
    }

    public AreaUpdateJson GetAreaUpdateJsonFor(PlayerCharacter player)
    {
        var allGameObjects = Array.Empty<GameObjectJson>()
            .Concat(_characters.ToJson())
            .Concat(_projectiles.ToJson())
            .Concat(_lootChests.ToJson())
            .ToList();
        return new(allGameObjects);
    }

    public void Update()
    {
        _timers.ForEach(t => t.Update());
        _characters.Update();
        _projectiles.Update();
        _lootChests.Update();

        // need to check for exit before map collisions, as players would get shoved out of exit
        foreach (var player in _players.AsEnumerable())
        {
            var firstExit = _exits.FirstOrDefault(x => x.ShouldExit(player, this));
            if (firstExit != null)
            {
                var newArea = _game.GetAreaByName(firstExit.DestinationName) ?? throw new Exception($"Invalid destination name: {firstExit.DestinationName}");
                RemovePlayer(player);
                newArea.SpawnPlayer(player);
                firstExit.OnExit(player, newArea);
            }
        }

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