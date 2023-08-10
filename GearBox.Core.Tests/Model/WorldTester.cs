namespace GearBox.Core.Tests.Model;

using GearBox.Core.Model;
using Xunit;

public class WorldTester
{
    [Fact]
    public void TwoWorldsWithSameIdAreEqual()
    {
        var world1 = new World();
        var world2 = new World(world1.Id);
        Assert.Equal(world1, world2);
    }

    [Fact]
    public void TwoWorldsWithDifferentIdAreNotEqual()
    {
        var world1 = new World();
        var world2 = new World();
        Assert.NotEqual(world1, world2);
    }

    [Fact]
    public void UpdateForwardsToAllDynamicObjects()
    {
        var spy = new DynamicGameObjectSpy();
        var sut = new World();
        sut.AddDynamicObject(spy);
        sut.Update();
        Assert.True(spy.HasBeenUpdated);
    }
}