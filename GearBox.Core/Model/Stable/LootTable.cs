namespace GearBox.Core.Model.Stable;

/// <summary>
/// Used to determine what items players can get from LootChests
/// </summary>
public class LootTable
{
    private readonly List<Func<IItem>> _itemDefinitions = new ();

    public void Add(Func<IItem> itemDefinition)
    {
        _itemDefinitions.Add(itemDefinition);
    }

    public IItem GetRandomItem()
    {
        if (!_itemDefinitions.Any())
        {
            throw new InvalidOperationException($"LootTable has no items");
        }

        var random = new Random();
        var i = random.Next(_itemDefinitions.Count);
        var result = _itemDefinitions[i].Invoke();
        return result;
    }
}