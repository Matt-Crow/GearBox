namespace GearBox.Core.Model.Items;

/// <summary>
/// Each piece of equipment goes in a specific slot.
/// It wouldn't make sense to equip an armor in your weapon slot!
/// </summary>
public class EquipmentSlotType
{
    public static readonly EquipmentSlotType WEAPON = new("Weapon");
    public static readonly EquipmentSlotType ARMOR = new("Armor");

    public static readonly IEnumerable<EquipmentSlotType> ALL = [WEAPON, ARMOR];


    private EquipmentSlotType(string name)
    {
        Name = name;
    }


    public string Name { get; init; }


    public static EquipmentSlotType? GetEquipmentSlotTypeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);
}