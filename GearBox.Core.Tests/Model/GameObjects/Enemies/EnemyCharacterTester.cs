using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.GameObjects.Enemies.Ai;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Enemies;

public class EnemyCharacterTester
{
    [Fact]
    public void Constructor_DefaultsToNullAI()
    {
        var sut = new EnemyCharacter("foo");
        Assert.IsAssignableFrom<NullAiBehavior>(sut.AiBehavior);
    }
}