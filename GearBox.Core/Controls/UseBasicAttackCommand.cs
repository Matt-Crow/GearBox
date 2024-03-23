using GearBox.Core.Model;
using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Controls;

public class UseBasicAttackCommand : IControlCommand
{
    private readonly Direction _inDirection;

    public UseBasicAttackCommand(Direction inDirection)
    {
        _inDirection = inDirection;
    }

    public void ExecuteOn(PlayerCharacter target, World inWorld)
    {
        target.UseBasicAttack(inWorld, _inDirection);
    }
}