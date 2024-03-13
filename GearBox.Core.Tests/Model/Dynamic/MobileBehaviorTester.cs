namespace GearBox.Core.Tests.Model.Dynamic;

using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Units;
using Xunit;

public class MobileBehaviorTester
{
    [Fact]
    public void ConstructorSetsReasonableDefaults()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.UP));
        Assert.False(sut.IsMoving);
        Assert.Equal(Coordinates.ORIGIN.CenteredOnTile(), sut.Coordinates);
    }

    [Fact]
    public void UpdateMovementWhenNotMovingDoesNotChangeCoordinates()
    {
        var expected = Coordinates.FromPixels(42, 42);
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.LEFT))
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
        var speed = Speed.FromTilesPerSecond(1);
        var sut = new MobileBehavior(Velocity.FromPolar(speed, Direction.RIGHT))
        {
            Coordinates = Coordinates.FromPixels(42, 42),
            IsMoving = true
        };

        sut.UpdateMovement();

        var expected = Coordinates.FromPixels((int)(42 + speed.InPixelsPerFrame), 42);
        Assert.Equal(expected, sut.Coordinates);
    }

    [Fact]
    public void StartMovingIn_GivenNotMoving_SetsVelocity()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT))
        {
            IsMoving = false
        };

        sut.StartMovingIn(Direction.UP);

        Assert.Equal(Direction.UP, sut.Velocity.Angle);
    }

    [Fact]
    public void StartMovingIn_GivenOrthogonal_Combines()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));

        sut.StartMovingIn(Direction.UP);
        sut.StartMovingIn(Direction.RIGHT);

        Assert.Equal(Direction.FromBearingDegrees(45), sut.Velocity.Angle);
    }

    [Fact]
    public void StartMovingIn_GivenOrthogonal_Combines2()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));

        sut.StartMovingIn(Direction.UP);
        sut.StartMovingIn(Direction.LEFT);

        Assert.Equal(Direction.FromBearingDegrees(315), sut.Velocity.Angle);
    }

    [Fact]
    public void StartMovingIn_GivenOpposite_Cancels()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));

        sut.StartMovingIn(Direction.UP);
        sut.StartMovingIn(Direction.DOWN);

        Assert.False(sut.IsMoving);
    }

    [Fact]
    public void StartMovingIn_GivenNeitherOrthogonalNorOpposite_Sets()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));
        var expected = Direction.FromBearingDegrees(42);
        sut.StartMovingIn(Direction.UP);
        sut.StartMovingIn(expected);

        Assert.Equal(expected, sut.Velocity.Angle);
    }

    [Fact]
    public void StopMovingIn_GivenNotMovingInThatDirection_DoesNotStop()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));
        sut.StartMovingIn(Direction.UP);

        sut.StopMovingIn(Direction.LEFT);

        Assert.True(sut.IsMoving);
    }

    [Fact]
    public void StopMovingIn_GivenMovingInThatDirection_Stops()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));
        sut.StartMovingIn(Direction.UP);

        sut.StopMovingIn(Direction.UP);

        Assert.False(sut.IsMoving);
    }

    [Fact]
    public void StopMovingIn_GivenMovingAtAngle_KeepsMovingInOther()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));
        sut.StartMovingIn(Direction.UP);
        sut.StartMovingIn(Direction.RIGHT);

        sut.StopMovingIn(Direction.UP);

        Assert.True(sut.IsMoving);
        Assert.Equal(Direction.RIGHT, sut.Velocity.Angle);
    }

    [Fact]
    public void StopMovingIn_GivenMovingAtAngle_KeepsMovingInOther2()
    {
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), Direction.RIGHT));
        sut.StartMovingIn(Direction.DOWN);
        sut.StartMovingIn(Direction.LEFT);

        sut.StopMovingIn(Direction.LEFT);

        Assert.True(sut.IsMoving);
        Assert.Equal(Direction.DOWN, sut.Velocity.Angle);
    }

    [Fact]
    public void StopMovingIn_GivenUnrelatedDirection_DoesNothing()
    {
        var expected = Direction.Between(Direction.DOWN, Direction.LEFT);
        var sut = new MobileBehavior(Velocity.FromPolar(Speed.FromTilesPerSecond(1), expected))
        {
            IsMoving = true
        };
        
        sut.StopMovingIn(Direction.UP);

        Assert.True(sut.IsMoving);
        Assert.Equal(expected, sut.Velocity.Angle);
    }
}