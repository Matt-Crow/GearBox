using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class ItemStackJson
{
    public required string ItemName { get; set; }
    public required int Quantity { get; set; }

    public ItemStackDTO ToItemStackDTO()
    {
        var result = new ItemStackDTO(ItemName, Quantity);
        return result;
    }
}