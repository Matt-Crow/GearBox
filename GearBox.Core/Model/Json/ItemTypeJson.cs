namespace GearBox.Core.Model.Json;

/// <summary>
/// Since some data does not change between items of the same type,
/// it is more efficient to pass that data to the front end once instead of for each item
/// </summary>
public readonly struct ItemTypeJson : IJson
{
    public ItemTypeJson(string name, int gradeOrder, string gradeName)
    {
        Name = name;
        GradeOrder = gradeOrder;
        GradeName = gradeName;
    }

    public string Name { get; init; }
    public int GradeOrder { get; init; }
    public string GradeName { get; init; }
}