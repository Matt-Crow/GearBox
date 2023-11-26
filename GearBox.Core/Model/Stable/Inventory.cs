using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IStableGameObject
{
    private readonly Dictionary<string, List<InventoryItem>> _content = new();


    public IEnumerable<InventoryItem> Content => _content.SelectMany(kv => kv.Value);

    public string Type => "inventory";

    public IEnumerable<object?> DynamicValues => Content;

    public void Add(InventoryItem item)
    {
        // do items need names?
    }

    public void Update()
    {
        // does nothing
    }

    public string Serialize(JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}