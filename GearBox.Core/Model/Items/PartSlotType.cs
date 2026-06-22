namespace GearBox.Core.Model.Items;

/// <summary>
/// Each robot part goes in a specific slot.
/// It wouldn't make sense to install a circle in your square slot!
/// </summary>
public class PartSlotType
{
    public static readonly IEnumerable<PartSlotType> ALL = [
        new("Head"),
        new("Locomotion"),
        new("Manipulator"), 
        new("Torso")
    ];


    private PartSlotType(string name)
    {
        Name = name;
    }


    public string Name { get; init; }

    public static PartSlotType? GetPartSlotTypeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);
}