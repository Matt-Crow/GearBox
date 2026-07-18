using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Utils;
using GearBox.Core.Utils.Factories;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Enemies;

public class EnemyFactoryTester
{
    [Fact]
    public void MakeRandom_GivenAiNotDisabled_ShouldNotHaveNullAi()
    {
        var config = new GearBoxConfig();
        var template = new EnemyCharacterTemplate("foo", Color.ALL.First(), []);
        var sut = new EnemyFactory(config, Factory<EnemyCharacterTemplate>.Of(ect => ect, [template]), new RandomNumberGenerator())
            .Add("foo");

        var result = sut.MakeRandom(1) ?? throw new Exception("Mock should be configured to return non-null");

        Assert.Null(result.AiBehavior as NullAiBehavior);
    }

    [Fact]
    public void MakeRandom_GivenAiDisabled_ShouldHaveNullAi()
    {
        var config = new GearBoxConfig()
        {
            DisableAI = true
        };
        var template = new EnemyCharacterTemplate("foo", Color.ALL.First(), []);
        var sut = new EnemyFactory(config, Factory<EnemyCharacterTemplate>.Of(ect => ect, [template]), new RandomNumberGenerator())
            .Add("foo");

        var result = sut.MakeRandom(1) ?? throw new Exception("Mock should be configured to return non-null");

        Assert.NotNull(result.AiBehavior as NullAiBehavior);
    }
}
