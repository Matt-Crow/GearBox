using GearBox.Core.Model.Json;
using GearBox.Core.Utils;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

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

    public IEnumerable<object?> DynamicValues => Content.Select(stack => stack.DynamicValues);

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

    public InventoryTabJson ToJson()
    {
        var items = _content.AsEnumerable()
            .Select(x => x.ToJson())
            .ToList();
        return new InventoryTabJson(items);
    }
}