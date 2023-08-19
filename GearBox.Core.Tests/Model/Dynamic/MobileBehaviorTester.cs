namespace GearBox.Core.Tests.Model.Dynamic;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Units;
using Xunit;

public class MobileBehaviorTester
{
    [Fact]
    public void ConstructorSetsReasonableDefaults()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.InTilesPerSecond(1), Direction.UP));
        Assert.False(sut.IsMoving);
        Assert.Equal(Coordinates.ORIGIN, sut.Coordinates);
    }

    [Fact]
    public void UpdateMovementWhenNotMovingDoesNotChangeCoordinates()
    {
        var expected = Coordinates.FromPixels(42, 42);
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.InTilesPerSecond(1), Direction.LEFT))
        {
            Coordinates = expected,
            IsMoving = false
        };

        sut.UpdateMovement();

        Assert.Equal(expected, sut.Coordinates);
    }

    [Fact]
    public void UpdateMovementAddsVelocityToCoordinates()
    {
        var speed = Speed.InTilesPerSecond(1);
        var sut = new MobileBehavior(Velocity.FromPolar(speed, Direction.RIGHT))
        {
            Coordinates = Coordinates.FromPixels(42, 42),
            IsMoving = true
        };

        sut.UpdateMovement();

        var expected = Coordinates.FromPixels((int)(42 + speed.InPixelsPerFrame), 42);
        Assert.Equal(expected, sut.Coordinates);
    }
}