using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class MaterialJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string Description { get; set; }

    public ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives)
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var result = ItemUnion.OfMaterial(new Material(Name, grade, Description));
        return result;
    }
}