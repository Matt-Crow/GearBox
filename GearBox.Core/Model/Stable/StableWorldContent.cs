namespace GearBox.Core.Model.Stable;

public class StableWorldContent
{
    private readonly List<IStableGameObject> _objects = new();
    private readonly List<PlayerCharacter> _players = new ();
    private readonly List<LootChest> _lootChests = new ();
    private readonly Dictionary<IStableGameObject, int> _hashes = new();
    private readonly List<Change> _pendingChanges = new();

    public void Add(IStableGameObject obj)
    {
        _objects.Add(obj);
        _hashes[obj] = MakeHashFor(obj);
        _pendingChanges.Add(Change.Created(obj));
    }

    // need this method, as there are special behaviors associated with players
    public void AddPlayer(PlayerCharacter player)
    {
        Add(player);
        _players.Add(player);
    }

    // player-interactables
    public void AddLootChest(LootChest lootChest)
    {
        Add(lootChest);
        _lootChests.Add(lootChest);
    }

    public IEnumerable<IStableGameObject> GetAll()
    {
        var result = _objects.AsEnumerable();
        return result;
    }

    private static int MakeHashFor(IStableGameObject obj)
    {
        var result = new HashCode();
        foreach (var field in obj.DynamicValues)
        {
            result.Add(field);
        }
        return result.ToHashCode();
    }

    /// <summary>
    /// Updates all dynamic objects
    /// </summary>
    /// <returns>all the changes which have occured since the last update</returns>
    public IEnumerable<Change> Update()
    {
        var result = new List<Change>();
        foreach (var obj in _objects)
        {
            obj.Update();
        }
        foreach (var lootChest in _lootChests)
        {
            foreach (var player in _players)
            {
                lootChest.CheckForCollisions(player);
            }
        }
        foreach (var obj in _objects.Where(HashHasChanged))
        {
            result.Add(Change.Updated(obj));
            _hashes[obj] = MakeHashFor(obj);
        }
        result.AddRange(_pendingChanges); // find any changes which occured during update
        _pendingChanges.Clear();
        return result;
    }

    private bool HashHasChanged(IStableGameObject obj)
    {
        return _hashes[obj] != MakeHashFor(obj);
    }
}