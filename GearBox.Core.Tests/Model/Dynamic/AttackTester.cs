using GearBox.Core.Model.Dynamic;
using Xunit;

namespace GearBox.Core.Tests.Model.Dynamic;

public class AttackTester
{
    [Fact]
    public void HandleCollision_CalledTwice_OnlyInflictsDamageOnce()
    {
        var sut = new Attack(new Character(), 42);
        var eventArgs = new CollideEventArgs(new Character());

        sut.HandleCollision(null, eventArgs);
        sut.HandleCollision(null, eventArgs);

        Assert.Equal(42, eventArgs.CollidedWith.DamageTaken);
    }
}