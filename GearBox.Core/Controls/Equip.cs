using GearBox.Core.Model.Stable;

namespace GearBox.Core.Controls;

public class Equip : IControlCommand
{
    private readonly Guid _equipmentId;

    public Equip(Guid equipmentId)
    {
        _equipmentId = equipmentId;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.EquipById(_equipmentId);
    }
}