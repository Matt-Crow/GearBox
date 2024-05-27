using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class EquipArmor : IControlCommand
{
    private readonly Guid _armorId;

    public EquipArmor(Guid armorId)
    {
        _armorId = armorId;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.EquipArmorById(_armorId);
    }
}