using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public interface IControlCommand
{
    void ExecuteOn(PlayerCharacter target);
}