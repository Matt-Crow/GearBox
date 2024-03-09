namespace GearBox.Core.Model.Stable;

public class Weapon : Equipment
{
    public Weapon(ItemType type, string? description = null, Guid? id = null) : base(type, description, id)
    {

    }

    public override EquipmentSlot GetSlot(PlayerCharacter player)
    {
        return player.Weapon;
    }
}