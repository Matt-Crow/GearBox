using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

public class ItemStack<T> : ISerializable<ItemJson>
where T : IItem
{
    public ItemStack(T item, int quantity=1)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        Item = item;
        Quantity = quantity;
    }

    public T Item { get; init; }
    public int Quantity { get; private set; }

    // Item.Id is immutable, but required to detect crafting
    public IEnumerable<object?> DynamicValues => [Item.Id, Quantity];

    public void AddQuantity(int quantity)
    {
        MustBeNonNegative(nameof(quantity), quantity);
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

    public ItemJson ToJson()
    {
        var result = new ItemJson(
            Item.Id,
            Item.Type.Name,
            Item.Description,
            Item.Level,
            Item.Details,
            Quantity
        );
        return result;
    }
}