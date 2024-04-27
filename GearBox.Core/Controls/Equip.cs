using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic.Player;

namespace GearBox.Core.Controls;

public class Equip : IControlCommand
{
    private readonly Guid _equipmentId;

    public Equip(Guid equipmentId)
    {
        _equipmentId = equipmentId;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.EquipWeaponById(_equipmentId);
    }
}