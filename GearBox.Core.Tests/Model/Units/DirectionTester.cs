namespace GearBox.Core.Tests.Model.Units;
using GearBox.Core.Model.Units;
using Xunit;

public class DirectionTester
{
    [Fact]
    public void FromBearing0IsUp()
    {
        var sut = Direction.FromBearingDegrees(0);
        var dx = sut.XMultiplier;
        var dy = sut.YMultiplier;

        Assert.Equal(0.0, Math.Round(dx, 3));
        Assert.Equal(-1.0, Math.Round(dy, 3));
    }

    [Fact]
    public void FromBearing90IsRight()
    {
        var sut = Direction.FromBearingDegrees(90);
        var dx = sut.XMultiplier;
        var dy = sut.YMultiplier;

        Assert.Equal(1.0, Math.Round(dx, 3));
        Assert.Equal(0, Math.Round(dy, 3));
    }

    [Theory]
    [InlineData(0, 45, 90)]
    [InlineData(180, 135, 90)]
    public void Between_ReturnsCorrect(int leg1, int diag, int leg2)
    {
        var angle1 = Direction.FromBearingDegrees(leg1);
        var angle2 = Direction.FromBearingDegrees(leg2);
        var expected = Direction.FromBearingDegrees(diag);

        var actual = Direction.Between(angle1, angle2);
        
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 45, 90)]
    [InlineData(180, 135, 90)]
    public void UnBetween_ReturnsCorrect(int leg1, int diag, int leg2)
    {
        var angle1 = Direction.FromBearingDegrees(leg1);
        var angle2 = Direction.FromBearingDegrees(leg2);
        var angle3 = Direction.FromBearingDegrees(diag);

        var actual = Direction.UnBetween(angle3, angle2);
        
        Assert.Equal(angle1, actual);
    }

    [Fact]
    public void DegreesBetween_LeftAndUp_Is90()
    {
        var actual = Direction.DegreesBetween(Direction.LEFT, Direction.UP);
        Assert.Equal(90, actual);
    }
}