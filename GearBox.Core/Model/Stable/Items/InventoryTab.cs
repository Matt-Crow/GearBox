using GearBox.Core.Model.Json;
using GearBox.Core.Utils;
using System.Text.Json;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Each Inventory is broken down into multiple tabs, which each hold a different category of item.
/// </summary>
public class InventoryTab : ISerializable<InventoryTabJson>
{
    /*
        While a List is less performant than a Dictionary for lookups, 
        I want to maintain insertion order
    */
    private readonly SafeList<ItemStack> _content = new();

    public IEnumerable<ItemStack> Content => _content.AsEnumerable();

    public IEnumerable<object?> DynamicValues => Content.SelectMany(stack => stack.DynamicValues);

    public void Add(IItem item, int quantity=1)
    {
        // check for existing stack, add to it if it exists
        var currentStack = _content.AsEnumerable()
            .Where(stack => stack.Item.Equals(item))
            .LastOrDefault();
        if (currentStack == null)
        {
            _content.Add(new ItemStack(item, quantity));
        }
        else
        {
            currentStack.AddQuantity(quantity);
        }
        _content.ApplyChanges();
    }

    public void AddRange(IEnumerable<IItem> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public void Remove(IItem item)
    {
        var stackToRemoveFrom = _content.AsEnumerable()
            .Where(stack => stack.Item.Equals(item))
            .FirstOrDefault();
        stackToRemoveFrom?.RemoveQuantity(1);
    }

    public bool Contains(IItem item)
    {
        var result = _content.AsEnumerable()
            .Any(stack => stack.Item.Equals(item) && stack.Quantity > 0);
        return result;
    }

    public IItem? GetItemById(Guid id)
    {
        var result = _content.AsEnumerable()
            .Where(stack => stack.Item.Id == id && stack.Quantity > 0)
            .Select(stack => stack.Item)
            .FirstOrDefault();
        return result;
    }

    public InventoryTabJson ToJson()
    {
        var items = _content.AsEnumerable()
            .Where(x => x.Quantity > 0)
            .Select(x => x.ToJson())
            .ToList();
        return new InventoryTabJson(items);
    }
}