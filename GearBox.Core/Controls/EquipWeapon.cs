using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class EquipWeapon : IControlCommand
{
    private readonly Guid _weaponId;

    public EquipWeapon(Guid weaponId)
    {
        _weaponId = weaponId;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.EquipWeaponById(_weaponId);
    }
}