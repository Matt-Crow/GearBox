using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable;

/// <summary>
/// Every item has a specific type, which is used to group it with similar items.
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