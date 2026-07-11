using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Web.Model.Json;

public interface IItemJson
{
    /// <summary>
    /// Converts this from JSON to an item,
    /// and uses the given lookups if required
    /// </summary>
    ItemUnion ToItem(Factory<IActiveAbility> actives, Factory<IPassiveAbility> passives);
}