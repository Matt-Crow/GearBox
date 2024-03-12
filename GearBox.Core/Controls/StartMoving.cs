using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Controls;

public class StartMoving : IControlCommand
{
    private readonly Direction _direction;
    public static readonly StartMoving UP = new(Direction.UP);
    public static readonly StartMoving DOWN = new(Direction.DOWN);
    public static readonly StartMoving LEFT = new(Direction.LEFT);
    public static readonly StartMoving RIGHT = new(Direction.RIGHT);

    private StartMoving(Direction direction)
    {
        _direction = direction;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.Inner.StartMovingIn(_direction);
    }
}