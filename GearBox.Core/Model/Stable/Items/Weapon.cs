namespace GearBox.Core.Model.Stable.Items;

public class Weapon : Equipment
{
    public Weapon(ItemType type, string? description = null, PlayerStatBoosts? statBoosts = null, Guid? id = null) : base(type, description, statBoosts, id)
    {
        
    }

    public override EquipmentSlot GetSlot(PlayerCharacter player)
    {
        return player.Weapon;
    }
}