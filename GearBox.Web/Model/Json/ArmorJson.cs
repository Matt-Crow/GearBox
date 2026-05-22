using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;

namespace GearBox.Web.Model.Json;

public class ArmorJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string ArmorClassName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];
    public List<string> PassiveNames { get; set; } = [];

    public ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives)
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var armorClass = ArmorClass.GetArmorClassByName(ArmorClassName) ?? throw new ArgumentException($"Invalid ArmorClassName: \"{ArmorClassName}\"", nameof(ArmorClass));
        var statDictionary = ItemJsonUtils.GetPlayerStats(Stats);
        var armor = new Equipment<ArmorStats>(
            Name, 
            new ArmorStats(armorClass), 
            grade, 
            statDictionary, 
            ItemJsonUtils.GetActives(actives, ActiveNames),
            ItemJsonUtils.GetPassives(passives, PassiveNames)
        );
        return ItemUnion.Of(armor);
    }
}
