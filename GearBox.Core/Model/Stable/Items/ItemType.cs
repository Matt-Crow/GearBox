using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

public readonly struct ItemType
{
    public ItemType(string name, Grade? grade = null)
    {
        Name = name;
        Grade = grade ?? Grade.COMMON;
    }

    public string Name { get; init; }
    public Grade Grade { get; init; }
    // todo add sprite once those are implemented

    public ItemTypeJson ToJson()
    {
        return new ItemTypeJson(Name);
    }
}