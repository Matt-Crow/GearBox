using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic.Player;

namespace GearBox.Core.Controls;

public class Respawn : IControlCommand
{
    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.HealPercent(100.0);
        inWorld.AddPlayer(target);
    }
}
