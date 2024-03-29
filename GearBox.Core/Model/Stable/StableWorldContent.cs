using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable;

public class StableWorldContent
{
    private readonly SafeList<IStableGameObject> _objects = new();
    private readonly SafeList<PlayerCharacter> _players = new ();
    private readonly SafeList<LootChest> _lootChests = new ();
    private readonly Dictionary<IStableGameObject, int> _hashes = new();
    private readonly List<Change> _added = new();

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

    public void Add(IStableGameObject obj)
    {
        _objects.Add(obj);
        _hashes[obj] = MakeHashFor(obj);
        _added.Add(Change.Content(obj));
    }

    private static int MakeHashFor(IStableGameObject obj)
    {
        var result = new HashCode();
        foreach (var field in obj.DynamicValues)
        {
            result.Add(field);
        }
        var hashCode = result.ToHashCode();
        return hashCode;
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
            _hashes[obj] = MakeHashFor(obj);
        }

        // notify caller of objects added during iteration
        result.AddRange(_added);
        _added.Clear();

        _objects.ApplyChanges();
        _players.ApplyChanges();
        _lootChests.ApplyChanges();

        return result;
    }

    private bool HashHasChanged(IStableGameObject obj)
    {
        return _hashes[obj] != MakeHashFor(obj);
    }
}