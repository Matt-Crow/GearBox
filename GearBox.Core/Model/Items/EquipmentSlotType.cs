namespace GearBox.Core.Model.Items;

/// <summary>
/// Each piece of equipment goes in a specific slot.
/// It wouldn't make sense to equip a torso in your manipulator slot!
/// </summary>
public class EquipmentSlotType
{
    public static readonly EquipmentSlotType MANIPULATOR = new("Manipulator", i => i.Manipulators);
    public static readonly EquipmentSlotType TORSO = new("Torso", i => i.Torsos);

    public static readonly IEnumerable<EquipmentSlotType> ALL = [MANIPULATOR, TORSO];

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