using GearBox.Core.Model.Json;
using GearBox.Core.Utils;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Each Inventory is broken down into multiple tabs, which each hold a different category of item.
/// </summary>
public class InventoryTab<T> : ISerializable<InventoryTabJson>
where T : IItem
{
    /*
        While a List is less performant than a Dictionary for lookups, 
        I want to maintain insertion order
    */
    private readonly SafeList<ItemStack<T>> _content = new();

    public IEnumerable<ItemStack<T>> Content => _content
        .AsEnumerable()
        .Where(stack => stack.Quantity > 0);

    public IEnumerable<object?> DynamicValues => Content.SelectMany(stack => stack.DynamicValues);

    public void Add(T? item, int quantity=1)
    {
        if (item == null || quantity <= 0)
        {
            return;
        }

        // check for existing stack, add to it if it exists
        var currentStack = _content.AsEnumerable()
            .Where(stack => stack.Item.Equals(item))
            .LastOrDefault();
        if (currentStack == null)
        {
            _content.Add(new ItemStack<T>(item, quantity));
        }
        else
        {
            currentStack.AddQuantity(quantity);
        }
        _content.ApplyChanges();
    }

    public bool Any()
    {
        var result = Content
            .Where(stack => stack.Quantity > 0)
            .Any();
        return result;
    }

    public void Remove(T item, int quantity=1)
    {
        var stackToRemoveFrom = _content.AsEnumerable()
            .Where(stack => stack.Item.Equals(item))
            .FirstOrDefault();
        stackToRemoveFrom?.RemoveQuantity(quantity);
    }

    public bool Contains(T item, int quantity=1)
    {
        var result = _content.AsEnumerable()
            .Any(stack => stack.Item.Equals(item) && stack.Quantity >= quantity);
        return result;
    }

    public T? GetItemById(Guid id)
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