namespace GearBox.Core.Model.Items;

/// <summary>
/// Each piece of equipment goes in a specific slot.
/// It wouldn't make sense to equip a circle in your square slot!
/// </summary>
public class EquipmentSlotType
{
    public static readonly IEnumerable<EquipmentSlotType> ALL = [
        new("Head"),
        new("Locomotion"),
        new("Manipulator"), 
        new("Torso")
    ];


    private EquipmentSlotType(string name)
    {
        Name = name;
    }


    public string Name { get; init; }

    public static EquipmentSlotType? GetEquipmentSlotTypeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);
}