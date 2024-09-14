
namespace GearBox.Core.Model.Json.AreaUpdate;

public class InventoryTabJson : IJson
{
    public InventoryTabJson(List<ItemJson> items)
    {
        Items = items;
    }

    public List<ItemJson> Items { get; init; }

    public IEnumerable<object?> DynamicValues => Items.SelectMany(i => i.DynamicValues);
}