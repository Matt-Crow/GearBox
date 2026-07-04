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

    public string ItemName { get; set; }
    public int Quantity { get; set; }
}