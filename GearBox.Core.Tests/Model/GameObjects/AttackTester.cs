using GearBox.Core.Model.GameObjects;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects;

public class AttackTester
{
    [Fact]
    public void HandleCollision_CalledTwice_OnlyInflictsDamageOnce()
    {
        var sut = new Attack(new Character("foo", 1), 42);
        var eventArgs = new CollideEventArgs(new Character("bar", 1));

        sut.HandleCollision(null, eventArgs);
        sut.HandleCollision(null, eventArgs);

        Assert.Equal(42, eventArgs.CollidedWith.DamageTaken);
    }
}