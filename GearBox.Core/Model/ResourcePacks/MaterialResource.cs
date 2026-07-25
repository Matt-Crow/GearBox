using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class MaterialResource : IItemResource
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string Description { get; set; }

    public ItemUnion ToItem(Factory<IActiveAbility> actives, Factory<IPassiveAbility> passives)
    {
        var grade = ItemResourceUtils.GetGradeByName(GradeName);
        var result = ItemUnion.OfMaterial(new Material(Name, grade, Description));
        return result;
    }
}