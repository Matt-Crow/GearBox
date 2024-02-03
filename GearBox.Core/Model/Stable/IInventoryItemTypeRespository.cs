namespace GearBox.Core.Model.Stable;

/// <summary>
/// Stores all inventory item type definitions
/// </summary>
public interface IInventoryItemTypeRepository
{
    /// <summary>
    /// Retrieves the item type definition with the given name, if such an item type exists.
    /// </summary>
    InventoryItemType? GetByName(string name);

    /// <summary>
    /// Retrieves all item type definitions
    /// </summary>
    IEnumerable<InventoryItemType> GetAll();
}