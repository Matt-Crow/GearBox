using GearBox.Core.Model.Areas;
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

namespace GearBox.Core.Model;

/// <summary>
/// For now, a World is the topmost container for game objects.
/// Future versions will need separate containers for the different game areas.
/// </summary>
public class World : IArea
{
    private readonly GameObjectCollection<IGameObject> _gameObjects = new();
    private readonly GameObjectCollection<Character> _characters = new();
    private readonly List<GameTimer> _timers = [];
    private readonly SafeList<LootChest> _lootChests = new(); 
    private readonly SafeList<PlayerCharacter> _players = new();
    private readonly Team _playerTeam = new("Players");
    private readonly Team _enemyTeam = new("Enemies");
    private readonly Map _map;
    private readonly IItemTypeRepository _itemTypes;
    private readonly CraftingRecipeRepository _craftingRecipes;
    private readonly LootTable _loot;
    private readonly List<Func<Character>> _enemyMakers;

    public World(
        Map? map = null, 
        IItemTypeRepository? itemTypes = null, 
        CraftingRecipeRepository? craftingRecipes = null,
        LootTable? loot = null,
        List<Func<Character>>? enemyMakers = null
    )
    {
        _map = map ?? new();
        _itemTypes = itemTypes ?? ItemTypeRepository.Empty();
        _craftingRecipes = craftingRecipes ?? CraftingRecipeRepository.Empty();
        _loot = loot ?? new LootTable();
        _enemyMakers = enemyMakers ?? [];

        if (_enemyMakers.Count == 0)
        {
            _enemyMakers.Add(() => new Character("Default enemy", 1));
        }
    }

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
        _lootChests.Add(lootChest);
        _gameObjects.AddGameObject(lootChest);
    }
    
    public void AddProjectile(Projectile projectile) => _gameObjects.AddGameObject(projectile);
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

    public CraftingRecipe? GetCraftingRecipeById(Guid id) => _craftingRecipes.GetById(id);

    public Character? GetNearestEnemy(Character character)
    {
        var result = _characters.GameObjects
            .Where(other => other.Team != character.Team)
            .OrderBy(enemy => enemy.Coordinates.DistanceFrom(character.Coordinates).InPixels)
            .FirstOrDefault();
        return result;
    }

    public Coordinates GetRandomFloorTile() => _map.GetRandomFloorTile();

    public AreaInitJson GetAreaInitJsonFor(PlayerCharacter player) => new(
        player.Id,
        _map.ToJson(),
        _itemTypes.ToJson(),
        _craftingRecipes.ToJson()
    );

    public AreaUpdateJson GetAreaUpdateJsonFor(PlayerCharacter player)
    {
        var allGameObjects = _gameObjects.ToJson()
            .Concat(_characters.ToJson())
            .ToList();
        return new(allGameObjects, player.GetChanges());
    }

    public void Update()
    {
        _timers.ForEach(t => t.Update());
        _gameObjects.Update();
        _characters.Update();
        foreach (var obj in _gameObjects.GameObjects)
        {
            // todo move Projectiles to their own list
            if (obj is Projectile projectile)
            {
                _map.CheckForCollisions(projectile);
            } else if (obj.Body != null)
            {
                _map.CheckForCollisions(obj.Body);
            }

            var body = obj.Body;
            if (body is not null)
            {
                CheckForCollisionsWithCharacters(body);
            }
        }
        foreach (var character in _characters.GameObjects)
        {
            _map.CheckForCollisions(character.Body);
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

    private void CheckForCollisionsWithCharacters(BodyBehavior body)
    {
        var collidingCharacters = _characters.GameObjects
            .Where(obj => obj.Body.CollidesWith(body));
        foreach (var character in collidingCharacters)
        {
            body.OnCollided(new CollideEventArgs(character));
        }
    }
}