namespace GearBox.Core.Model.Items;

/// <summary>
/// The raw data about an ItemStack
/// </summary>
public class ItemStackDTO
{
    public ItemStackDTO(string itemName, int quantity = 1)
    {
        ItemName = itemName;
        Quantity = quantity;    
    }

    public string ItemName { get; init; }
    public int Quantity { get; init; }
}