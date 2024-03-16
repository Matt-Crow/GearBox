using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable;

public class StableWorldContent
{
    private readonly DynamicHasher _hasher = new();
    private readonly SafeList<IStableGameObject> _objects = new();
    private readonly SafeList<PlayerCharacter> _players = new ();
    private readonly SafeList<LootChest> _lootChests = new ();
    private readonly Dictionary<IStableGameObject, int> _hashes = new();
    private readonly List<Change> _changes = new();

    // need this method, as there are special behaviors associated with players
    public void AddPlayer(PlayerCharacter player)
    {
        Add(player);
        _players.Add(player);
    }

    public void RemovePlayer(PlayerCharacter player)
    {
        Remove(player);
        _players.Remove(player);
    }

    // player-interactables
    public void AddLootChest(LootChest lootChest)
    {
        Add(lootChest);
        _lootChests.Add(lootChest);
    }

    public void Add(IStableGameObject obj)
    {
        _objects.Add(obj);
        _hashes[obj] = _hasher.Hash(obj);
        _changes.Add(Change.Content(obj));
    }

    public void Remove(IStableGameObject obj)
    {
        _objects.Remove(obj);
        _hashes.Remove(obj);
        _changes.Add(Change.Removed(obj));
    }

    public IEnumerable<IStableGameObject> GetAll()
    {
        var result = _objects.AsEnumerable();
        return result;
    }

    /// <summary>
    /// Updates all dynamic objects
    /// </summary>
    /// <returns>all the changes which have occured since the last update</returns>
    public IEnumerable<Change> Update()
    {
        var result = new List<Change>();
        foreach (var obj in _objects.AsEnumerable())
        {
            obj.Update(); 
        }
        foreach (var lootChest in _lootChests.AsEnumerable())
        {
            foreach (var player in _players.AsEnumerable())
            {
                lootChest.CheckForCollisions(player);
            }
        }

        foreach (var obj in _objects.AsEnumerable().Where(HashHasChanged))
        {
            result.Add(Change.Content(obj));
            _hashes[obj] = _hasher.Hash(obj);
        }

        // notify caller of objects added or removed during iteration
        result.AddRange(_changes);
        _changes.Clear();

        _objects.ApplyChanges();
        _players.ApplyChanges();
        _lootChests.ApplyChanges();

        return result;
    }

    private bool HashHasChanged(IStableGameObject obj)
    {
        return _hashes.ContainsKey(obj) && _hashes[obj] != _hasher.Hash(obj);
    }
}