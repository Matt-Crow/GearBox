namespace GearBox.Core.Model.Items;

/// <summary>
/// Each piece of equipment goes in a specific slot.
/// It wouldn't make sense to equip a torso in your manipulator slot!
/// </summary>
public class EquipmentSlotType
{
    public static readonly EquipmentSlotType MANIPULATOR = new("Manipulator");
    public static readonly EquipmentSlotType TORSO = new("Torso");

    public static readonly IEnumerable<EquipmentSlotType> ALL = [MANIPULATOR, TORSO];


    private EquipmentSlotType(string name)
    {
        Name = name;
    }


    public string Name { get; init; }

    public static EquipmentSlotType? GetEquipmentSlotTypeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);
}