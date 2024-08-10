using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.Items;

public class ArmorBuilder(ItemType type) : EquipmentBuilder<Armor>(type)
{
    private ArmorClass _armorClass = ArmorClass.NONE;
    
    public ArmorBuilder WithArmorClass(ArmorClass armorClass)
    {
        _armorClass = armorClass;
        return this;
    }

    public override Armor DoBuild(int level, Dictionary<PlayerStatType, int> statWeights)
    {
        var stats = new PlayerStatBoosts(statWeights, Armor.GetStatPoints(level, ItemType.Grade, _armorClass));
        var result = new Armor(
            ItemType,
            level,
            null, // id is null
            _armorClass,
            stats
        );
        return result;
    }
}