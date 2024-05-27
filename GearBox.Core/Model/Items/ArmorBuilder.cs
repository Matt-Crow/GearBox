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

    protected override int ModifyPoints(int points)
    {
        return (int)(points * _armorClass.ArmorStatMultiplier);
    }

    public override Armor DoBuild(int level, PlayerStatBoosts statBoosts)
    {
        var result = new Armor(
            ItemType,
            level,
            null, // id is null
            _armorClass,
            statBoosts
        );
        return result;
    }
}