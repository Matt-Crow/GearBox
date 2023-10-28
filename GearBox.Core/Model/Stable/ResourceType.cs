using System.Text.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// A Resource is a type of item which players can pick up.
/// In future updates, players will be able to use these to craft other items.
/// </summary>
public readonly struct ResourceType : IInventoryItemType
{
    public ResourceType(string type)
    {
        Name = type;
    }

    public string Name { get; init; }

    public string ItemType => "resource";
    
    public bool IsStackable => true;

    public string Serialize(JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(new ResourceTypeJson(this), options);
    }

    // since all Resources stack, don't bother serializing IsStackable
    public class ResourceTypeJson
    {
        public ResourceTypeJson(ResourceType asStruct)
        {
            Name = asStruct.Name;
        }

        public string Name { get; init; }
    }
}