using GearBox.Core.Controls;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Tests.Server;

public class SpyControlCommand : IControlCommand
{
    public bool HasBeenExecuted { get; private set; }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        HasBeenExecuted = true;
    }
}
