namespace GearBox.Core.Model.Stable.Items;

public class Weapon : Equipment
{
    private readonly WeaponStats _stats;
    
    public Weapon(ItemType type, string? description = null, Guid? id = null, WeaponStats? stats = null) : base(type, description, stats?.PlayerStatBoosts, id)
    {
        _stats = stats ?? new WeaponStats(0, StatBoosts);
    }

    public override EquipmentSlot GetSlot(PlayerCharacter player)
    {
        return player.Weapon;
    }
}