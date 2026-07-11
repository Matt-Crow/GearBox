using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils.Factories;

namespace GearBox.Core.Model.ResourcePacks;

public class PartResource : IItemResource
{
    public required string Name { get; set; }
    public required string Slot { get; set; }
    public required string GradeName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];
    public List<string> PassiveNames { get; set; } = [];


    public ItemUnion ToItem(Factory<IActiveAbility> actives, Factory<IPassiveAbility> passives)
    {
        var slotType = PartSlotType.GetPartSlotTypeByName(Slot) ?? throw new Exception($"Invalid slot type: \"{Slot}\"");
        var grade = ItemResourceUtils.GetGradeByName(GradeName);
        var statDictionary = ItemResourceUtils.GetPlayerStats(Stats);
        var part = new Part(
            Name, 
            slotType,
            grade, 
            statDictionary, 
            ItemResourceUtils.GetActives(actives, ActiveNames),
            ItemResourceUtils.GetPassives(passives, PassiveNames)
        );

        return ItemUnion.OfPart(part);
    }
}