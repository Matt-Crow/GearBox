using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public interface IItemJson
{
    /// <summary>
    /// Converts this from JSON to an item,
    /// and uses the given factories if required
    /// </summary>
    ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives);
}