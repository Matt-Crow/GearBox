using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

public class Respawn : IControlCommand
{
    private readonly IArea _area;

    public Respawn(IArea area)
    {
        _area = area;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        _area.SpawnPlayer(target);
    }
}
