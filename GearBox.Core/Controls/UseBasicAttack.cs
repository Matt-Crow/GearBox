using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Controls;

public class UseBasicAttack : IControlCommand
{
    private readonly Direction _inDirection;

    public UseBasicAttack(Direction inDirection)
    {
        _inDirection = inDirection;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.UseBasicAttack(inWorld, _inDirection);
    }
}