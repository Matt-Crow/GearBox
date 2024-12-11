using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Units;

namespace GearBox.Web.Model.Json;

public class WeaponJson : IItemJson
{
    public required string Name { get; set; }
    public required string GradeName { get; set; }
    public required string RangeName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];

    public ItemUnion ToItem(IActiveAbilityFactory actives)
    {
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var attackRange = AttackRange.GetAttackRangeByName(RangeName) ?? throw new ArgumentException($"Invalid RangeName: \"{RangeName}\"", nameof(RangeName));
        var statDictionary = ItemJsonUtils.GetPlayerStats(Stats);
        var weapon = new Equipment<WeaponStats>(Name, new WeaponStats(attackRange), grade, statDictionary, ItemJsonUtils.GetActives(actives, ActiveNames));
        return ItemUnion.Of(weapon);
    }
}