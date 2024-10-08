using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Units;

namespace GearBox.Core.Controls;

public class StopMoving : IControlCommand
{
    private readonly Direction _direction;
    public static readonly StopMoving UP = new(Direction.UP);
    public static readonly StopMoving DOWN = new(Direction.DOWN);
    public static readonly StopMoving LEFT = new(Direction.LEFT);
    public static readonly StopMoving RIGHT = new(Direction.RIGHT);

    private StopMoving(Direction direction)
    {
        _direction = direction;
    }

    public void ExecuteOn(PlayerCharacter target)
    {
        target.StopMovingIn(_direction);
    }
}