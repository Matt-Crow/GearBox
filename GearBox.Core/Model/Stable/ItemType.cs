using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Every item has a specific type, which is used to group it with similar items.
/// This is currently mostly useless, but I may revise it in the future to add an image for the item
/// </summary>
public readonly struct ItemType
{
    public ItemType(string name)
    {
        Name = name;
    }

    public string Name { get; init; }

    public ItemTypeJson ToJson()
    {
        return new ItemTypeJson(Name);
    }
}