using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class WeaponJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];
    public List<string> PassiveNames { get; set; } = [];


    public ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives)
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var statDictionary = ItemJsonUtils.GetPlayerStats(Stats);
        var weapon = new Equipment(
            Name, 
            grade, 
            statDictionary, 
            ItemJsonUtils.GetActives(actives, ActiveNames),
            ItemJsonUtils.GetPassives(passives, PassiveNames)
        );
        return ItemUnion.OfWeapon(weapon);
    }
}