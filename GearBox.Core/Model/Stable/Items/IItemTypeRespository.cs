using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Stable.Items;

/// <summary>
/// Stores all item type definitions
/// </summary>
public interface IItemTypeRepository
{
    /// <summary>
    /// Retrieves the item type definition with the given name, if such an item type exists.
    /// </summary>
    ItemType? GetByName(string name);

    /// <summary>
    /// Retrieves all item type definitions
    /// </summary>
    IEnumerable<ItemType> GetAll();

    /// <summary>
    /// Gets the contents of the repository as JSON
    /// </summary>
    List<ItemTypeJson> ToJson();
}