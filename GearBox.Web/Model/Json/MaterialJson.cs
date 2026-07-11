using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Lookups;

namespace GearBox.Web.Model.Json;

public class MaterialJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string Description { get; set; }

    public ItemUnion ToItem(Lookup<IActiveAbility> actives, Lookup<IPassiveAbility> passives)
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var result = ItemUnion.OfMaterial(new Material(Name, grade, Description));
        return result;
    }
}