using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class Respawn : IControlCommand
{
    public void ExecuteOn(PlayerCharacter target)
    {
        target.LastArea?.SpawnPlayer(target);
    }
}
