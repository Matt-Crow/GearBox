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
}