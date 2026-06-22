using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class PartTester
{
    [Fact]
    public void ToOwned_GivenHigherLevel_HasHigherStats()
    {
        var stat = PlayerStatType.MAX_HIT_POINTS;
        var weights = new Dictionary<PlayerStatType, int>()
        {
            {stat, 1}
        };
        var sut = new Part("foo", PartSlotType.ALL.First(), statWeights: weights, level: 1);

        var other = sut.ToOwned(2);

        int GetStat(Part w) => w.StatBoosts.Get(stat);
        Assert.True(GetStat(other) > GetStat(sut));
    }
}