using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class EquipmentTester
{
    [Fact]
    public void ToOwned_GivenHigherLevel_HasHigherStats()
    {
        var stat = PlayerStatType.MAX_HIT_POINTS;
        var weights = new Dictionary<PlayerStatType, int>()
        {
            {stat, 1}
        };
        var sut = new Equipment<WeaponStats>("foo", new WeaponStats(), statWeights: weights, level: 1);

        var other = sut.ToOwned(2);

        int GetStat(Equipment<WeaponStats> w) => w.StatBoosts.Get(stat);
        Assert.True(GetStat(other) > GetStat(sut));
    }
}