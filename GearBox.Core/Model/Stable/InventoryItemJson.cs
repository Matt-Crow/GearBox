using System.Text.Json;

namespace GearBox.Core.Model.Stable;

public class InventoryItemJson : IJson
{
    public InventoryItemJson(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }

    public string Name { get; init; }
    public int Quantity { get; init; }
    // TODO non-stackable items have metadata?
    // TODO non-stackable items tags?
}