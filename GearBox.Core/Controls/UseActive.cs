using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Controls;

public class UseActive : IControlCommand
{
    private readonly int _number;
    private readonly Direction _inDirection;

    public UseActive(int number, Direction inDirection)
    {
        _number = number;
        _inDirection = inDirection;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.UseActive(_number, _inDirection);
    }
}
