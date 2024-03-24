using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Stable;

public class StableWorldContent
{
    private readonly SafeList<IStableGameObject> _objects = new();
    private readonly SafeList<PlayerCharacter> _players = new ();
    private readonly SafeList<LootChest> _lootChests = new ();
    private readonly Dictionary<IStableGameObject, ChangeTracker> _changeTrackers = [];


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
        _changeTrackers.Add(obj, new ChangeTracker(obj));
    }

    public void Remove(IStableGameObject obj)
    {
        _objects.Remove(obj);
        _changeTrackers[obj].OnRemoved();
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

        // notify caller of objects added, removed, or changed during iteration
        var result = _changeTrackers.Values
            .Where(x => x.HasChanged)
            .Select(x => x.State)
            .ToList(); // important! ToList must come before OnUpdateEnded

        var pendingRemove = new List<IStableGameObject>();
        foreach (var changeTracker in _changeTrackers)
        {
            if (changeTracker.Value.State.IsDelete)
            {
                pendingRemove.Add(changeTracker.Key);
            }
            changeTracker.Value.OnUpdateEnd();
        }
        foreach (var gameObject in pendingRemove)
        {
            _changeTrackers.Remove(gameObject);
        }
        
        _objects.ApplyChanges();
        _players.ApplyChanges();
        _lootChests.ApplyChanges();

        return result;
    }
}