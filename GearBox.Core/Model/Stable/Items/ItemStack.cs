using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

public class ItemStack<T> : ISerializable<ItemJson>
where T : IItem
{
    public ItemStack(IItem item, int quantity=1)
    {
        MustBeNonNegative(nameof(quantity), quantity);
        Item = item;
        Quantity = quantity;
    }

    public IItem Item { get; init; }
    public int Quantity { get; private set; }
    public IEnumerable<object?> DynamicValues => Item.DynamicValues.Append(Quantity);

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