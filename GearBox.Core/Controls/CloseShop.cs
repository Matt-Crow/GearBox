using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class CloseShop : IControlCommand
{
    public void ExecuteOn(PlayerCharacter target)
    {
        target.SetOpenShop(null);
    }
}
