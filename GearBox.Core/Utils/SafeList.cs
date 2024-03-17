//using System.Collections;

namespace GearBox.Core.Utils;

/// <summary>
/// It is safe to mutate the contents of a SafeList while iterating over it.
/// Changes to it do not apply until you call ApplyChanges.
/// 
/// Note that this class should not inherit from ICollection,
/// as the C# runtime will incorrectly detect concurrent modification.
/// </summary>
public class SafeList<T>
{
    private readonly List<T> _pendingAdd = new();
    private readonly List<T> _pendingRemove = new();
    private readonly List<T> _items = new();

    public SafeList(params T[] items)
    {
        AddRange(items);
        ApplyChanges();
    }

    /// <summary>
    /// The number of items currently in the list, excluding pending changes
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Adds the item after the next call to ApplyChanges
    /// </summary>
    public void Add(T item)
    {
        _pendingAdd.Add(item);
    }

    /// <summary>
    /// Adds the items after the next call to ApplyChanges
    /// </summary>
    public void AddRange(params T[] items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Adds the items after the next call to ApplyChanges
    /// </summary>
    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Checks whether the given item would exist after the next call to ApplyChanges
    /// </summary>
    public bool Contains(T obj)
    {
        var willBeInItems = _pendingAdd.Contains(obj) || _items.Contains(obj);
        var result = willBeInItems && !_pendingRemove.Contains(obj);
        return result;
    }

    /// <summary>
    /// Gets the current values in the list, excluding any pending changes
    /// </summary>
    public IEnumerable<T> AsEnumerable()
    {
        lock (_items)
        {
            // concurrent modification if I exclude ToList?
            return _items.ToList();
        }
    }

    /// <summary>
    /// Schedules the given item to be removed after the next call to ApplyChanges
    /// </summary>
    public void Remove(T item)
    {
        _pendingRemove.Add(item);
    }

    /// <summary>
    /// Applies all pending changes to this list
    /// </summary>
    public void ApplyChanges()
    {
        _items.AddRange(_pendingAdd);
        _pendingAdd.Clear();

        _items.RemoveAll(item => _pendingRemove.Contains(item));
        _pendingRemove.Clear();
    }
}