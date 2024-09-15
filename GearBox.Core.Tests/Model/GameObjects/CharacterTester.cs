using GearBox.Core.Model.GameObjects;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects;

public class CharacterTester
{
    [Fact]
    public void KilledEventIsOnlyRaisedOnce()
    {
        var timesCalled = 0;
        var sut = new ExampleCharacter();
        sut.Killed += (sender, args) => timesCalled++;

        sut.HandleAttacked(new(new Attack(new ExampleCharacter(), 9999), sut));
        sut.HandleAttacked(new(new Attack(new ExampleCharacter(), 9999), sut));

        Assert.Equal(1, timesCalled);
    }

    public class ExampleCharacter : Character
    {
        public ExampleCharacter() : base("foo", 1)
        {
        }
    }
}