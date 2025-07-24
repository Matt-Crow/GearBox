using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.GameObjects.Enemies;
using GearBox.Core.Model.GameObjects.Enemies.Ai;
using GearBox.Core.Model.Items;
using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects.Enemies;

public class EnemyFactoryTester
{
    [Fact]
    public void MakeRandom_GivenAiNotDisabled_ShouldNotHaveNullAi()
    {
        var config = new GearBoxConfig();
        var sut = new EnemyFactory(config, new EnemyRepositoryMock(), new RandomNumberGenerator())
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
        var sut = new EnemyFactory(config, new EnemyRepositoryMock(), new RandomNumberGenerator())
            .Add("foo");

        var result = sut.MakeRandom(1) ?? throw new Exception("Mock should be configured to return non-null");

        Assert.NotNull(result.AiBehavior as NullAiBehavior);
    }

    private class EnemyRepositoryMock : IEnemyRepository
    {
        public IEnemyRepository Add(string name, Color color, Func<LootTableBuilder, LootTableBuilder> loot) => this;

        public EnemyCharacter? GetEnemyByName(string name, int level) => new EnemyCharacter(name, level);
    }
}
