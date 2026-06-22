using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class PartJson : IItemJson
{
    public required string Name { get; set; }
    public required string Slot { get; set; }
    public required string GradeName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];
    public List<string> PassiveNames { get; set; } = [];


    public ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives)
    {
        var slotType = PartSlotType.GetPartSlotTypeByName(Slot) ?? throw new Exception($"Invalid slot type: \"{Slot}\"");
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var statDictionary = ItemJsonUtils.GetPlayerStats(Stats);
        var part = new Part(
            Name, 
            slotType,
            grade, 
            statDictionary, 
            ItemJsonUtils.GetActives(actives, ActiveNames),
            ItemJsonUtils.GetPassives(passives, PassiveNames)
        );

        return ItemUnion.OfPart(part);
    }
}