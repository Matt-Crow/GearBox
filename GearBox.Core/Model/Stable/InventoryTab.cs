using GearBox.Core.Model.Json;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Each Inventory is broken down into multiple tabs, which each hold a different category of item.
/// </summary>
public class InventoryTab : IStableGameObject, ISerializable<InventoryTabJson>
{
    /*
        While a List is less performant than a Dictionary for lookups, 
        I want to maintain insertion order
    */
    // TODO change to ItemStack
    private readonly List<Item> _content = new();

    public IEnumerable<Item> Content => _content.AsEnumerable();

    public string Type => "inventoryTab";

    public IEnumerable<object?> DynamicValues => Content;

    public void Add(Item item)
    {
        if (item.ItemType.IsStackable)
        {
            var currentStack = _content
                .Where(x => x.ItemType.Name == item.ItemType.Name)
                .LastOrDefault();
            
            if (currentStack == null)
            {
                _content.Add(item);
            }
            else
            {
                currentStack.AddQuantity(item.Quantity);
            }
        }
        else
        {
            _content.Add(item);
        }
    }

    public void Update()
    {
        // does nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public InventoryTabJson ToJson()
    {
        var items = Content
            .Select(x => x.ToJson())
            .ToList();
        return new InventoryTabJson(items);
    }
}