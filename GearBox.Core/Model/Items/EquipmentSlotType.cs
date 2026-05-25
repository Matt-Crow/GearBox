namespace GearBox.Core.Model.Items;

/// <summary>
/// Each piece of equipment goes in a specific slot.
/// It wouldn't make sense to equip an armor in your weapon slot!
/// </summary>
public class EquipmentSlotType
{
    public static readonly EquipmentSlotType WEAPON = new("Weapon", i => i.Weapons);
    public static readonly EquipmentSlotType ARMOR = new("Armor", i => i.Armors);

    public static readonly IEnumerable<EquipmentSlotType> ALL = [WEAPON, ARMOR];

    private readonly Func<Inventory, InventoryTab<Equipment>> _getInventoryTab;


    private EquipmentSlotType(string name, Func<Inventory, InventoryTab<Equipment>> getInventoryTab)
    {
        Name = name;
        _getInventoryTab = getInventoryTab;
    }


    public string Name { get; init; }


    /// <summary>
    /// Gets the tab in the given inventory where this type of equipment belongs
    /// </summary>
    public InventoryTab<Equipment> GetInventoryTab(Inventory inventory) => _getInventoryTab(inventory);


    public static EquipmentSlotType? GetEquipmentSlotTypeByName(string name) => ALL.FirstOrDefault(x => x.Name == name);
}