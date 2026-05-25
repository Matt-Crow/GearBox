using GearBox.Core.Model.Abilities.Actives;
using GearBox.Core.Model.Abilities.Passives;
using GearBox.Core.Model.Items;

namespace GearBox.Web.Model.Json;

public class EquipmentJson : IItemJson
{
    public required string Name { get; set; }
    public required string Slot { get; set; }
    public required string GradeName { get; set; }
    public required Dictionary<string, int> Stats { get; set; }
    public List<string> ActiveNames { get; set; } = [];
    public List<string> PassiveNames { get; set; } = [];


    public ItemUnion ToItem(IActiveAbilityFactory actives, IPassiveAbilityFactory passives)
    {
        var slotType = EquipmentSlotType.GetEquipmentSlotTypeByName(Slot) ?? throw new Exception($"Invalid slot type: \"{Slot}\"");
        var grade = ItemJsonUtils.GetGradeByName(GradeName);
        var statDictionary = ItemJsonUtils.GetPlayerStats(Stats);
        var equipment = new Equipment(
            Name, 
            slotType,
            grade, 
            statDictionary, 
            ItemJsonUtils.GetActives(actives, ActiveNames),
            ItemJsonUtils.GetPassives(passives, PassiveNames)
        );

        if (Slot == "Weapon")
        {
            return ItemUnion.OfWeapon(equipment);
        }

        if (Slot == "Armor")
        {
            return ItemUnion.OfArmor(equipment);
        }

        throw new Exception($"Invalid slot: \"{Slot}\"");
    }
}