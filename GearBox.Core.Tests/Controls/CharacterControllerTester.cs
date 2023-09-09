using GearBox.Core.Controls;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Units;
using Xunit;

namespace GearBox.Core.Tests.Controls;

public class CharacterControllerTester
{
    [Fact]
    public void Receive_GivenStartMoving_SetsVelocityAngle()
    {
        var target = new Character()
        {
            Coordinates = Coordinates.ORIGIN
        };
        var sut = new CharacterController(target);

        sut.Receive(StartMoving.RIGHT);
        target.Update();

        Assert.True(target.Coordinates.XInPixels > 0);
        Assert.True(target.Coordinates.YInPixels == 0);
    }
}