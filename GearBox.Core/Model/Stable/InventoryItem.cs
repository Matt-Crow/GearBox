using GearBox.Core.Model.Json;
using System.Collections.Immutable;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// an item which can go in a player's inventory
/// </summary>
public class InventoryItem : IStableGameObject, ISerializable<InventoryItemJson>
{
    public InventoryItem(InventoryItemType type) : this(type, 1)
    {

    }

    public InventoryItem(InventoryItemType type, int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        MustBeValidQuantity(type, quantity);
        ItemType = type;
        Quantity = quantity;
    }

    public string Type => "inventoryItem";
    
    public InventoryItemType ItemType {get; init; }

    /// <summary>
    /// Subclasses should override this method if they need to provide metadata to the front end
    /// </summary>
    protected virtual List<KeyValueJson<string, object?>> Metadata => new();
    
    /// <summary>
    /// Subclasses should override this method if they need to provide tags to the front end
    /// </summary>
    protected virtual List<string> Tags => new();

    public int Quantity { get; private set; }

    public IEnumerable<object?> DynamicValues => ImmutableArray.Create<object?>(Quantity);

    public void AddQuantity(int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        MustBeValidQuantity(ItemType, Quantity + quantity);
        Quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        if (Quantity < quantity)
        {
            throw new Exception($"Item only has {Quantity} quantity, so cannot remove {quantity}");
        }
        Quantity -= quantity;
    }

    private static void MustBeNonNegative(string name, int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(name, $"Must be non-negative, so {value} is not allowed.");
        }
    }
    private static void MustBeValidQuantity(InventoryItemType type, int quantity)
    {
        if (!type.IsStackable && quantity != 0 && quantity != 1)
        {
            throw new ArgumentOutOfRangeException($"{type.Name} is not stackable, so quantity must be 0 or 1, not {quantity}");
        }
    }

    public void Update()
    {
        // does nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(ToJson(), options);
    }

    public InventoryItemJson ToJson()
    {
        return new InventoryItemJson(ItemType.Name, Metadata, Tags, Quantity);
    }
}