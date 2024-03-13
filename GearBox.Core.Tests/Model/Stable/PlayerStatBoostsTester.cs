using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class PlayerStatBoostsTester
{
    [Fact]
    public void Add_ModifiesThis()
    {
        var sut = new PlayerStatBoosts();

        var other = sut.Add(PlayerStatType.MAX_HIT_POINTS, 1);

        Assert.Equal(sut, other);
    }

    [Fact]
    public void Add_CalledMultipleTimes_Sums()
    {
        var sut = new PlayerStatBoosts();

        sut.Add(PlayerStatType.MAX_ENERGY, 1);
        sut.Add(PlayerStatType.MAX_ENERGY, 2);

        Assert.Equal(3, sut.Get(PlayerStatType.MAX_ENERGY));
    }

    [Fact]
    public void Add_GivenPlayerStatBoosts_WorksAsExpected()
    {
        var sut = new PlayerStatBoosts()
            .Add(PlayerStatType.OFFENSE, 1)
            .Add(PlayerStatType.DEFENSE, 2);
        var other = new PlayerStatBoosts()
            .Add(PlayerStatType.OFFENSE, 3)
            .Add(PlayerStatType.SPEED, 5);
        
        sut.Add(other);

        Assert.Equal(4, sut.Get(PlayerStatType.OFFENSE));
        Assert.Equal(2, sut.Get(PlayerStatType.DEFENSE));
        Assert.Equal(5, sut.Get(PlayerStatType.SPEED));
    }
}