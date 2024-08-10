using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class ArmorTester
{
    [Fact]
    public void ToOwned_GivenHigherLevel_HasHigherStats()
    {
        var stat = PlayerStatType.MAX_HIT_POINTS;
        var boosts = new PlayerStatBoosts(new Dictionary<PlayerStatType, int>()
        {
            {stat, 1}
        });
        var sut = new Armor(new ItemType("foo"), level: 1, boosts: boosts);

        var other = sut.ToOwned(2);

        int GetStat(Armor w) => w.StatBoosts.Get(stat);
        Assert.True(GetStat(other) > GetStat(sut));
    }
}