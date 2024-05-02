using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic.Player;

namespace GearBox.Core.Controls;

public class EquipWeapon : IControlCommand
{
    private readonly Guid _weaponId;

    public EquipWeapon(Guid weaponId)
    {
        _weaponId = weaponId;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.EquipWeaponById(_weaponId);
    }
}