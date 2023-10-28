using System.Collections.Immutable;
using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// an item which can go in a player's inventory
/// </summary>
public class InventoryItem : IStableGameObject
{
    public InventoryItem(IInventoryItemType inner) : this(inner, 1)
    {

    }

    public InventoryItem(IInventoryItemType inner, int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        MustBeValidQuantity(inner, quantity);
        Inner = inner;
        Quantity = quantity;
    }

    public string Type { get => "inventoryItem"; }

    public IInventoryItemType Inner {get; init; }

    public int Quantity { get; private set; }

    public IEnumerable<object?> DynamicValues { get => ImmutableArray.Create<object?>(Quantity); }

    public void AddQuantity(int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        MustBeValidQuantity(Inner, Quantity + quantity);
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
    private static void MustBeValidQuantity(IInventoryItemType type, int quantity)
    {
        if (!type.IsStackable && quantity != 0 && quantity != 1)
        {
            throw new ArgumentOutOfRangeException($"{type.ItemType} is not stackable, so quantity must be 0 or 1, not {quantity}");
        }
    }

    public void Update()
    {
        // does nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(new InventoryItemJson(this, options), options);
    }

    // only need to serialize a few properties
    public class InventoryItemJson
    {
        public InventoryItemJson(InventoryItem item, JsonSerializerOptions options)
        {
            Inner = item.Inner.Serialize(options);
            InnerType = item.Inner.ItemType;
            Quantity = item.Quantity;
        }

        public string Inner { get; init; }
        public string InnerType { get; init; }
        public int Quantity { get; init; }
    }
}