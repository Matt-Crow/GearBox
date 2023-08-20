namespace GearBox.Core.Tests.Model;

using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Static;
using GearBox.Core.Tests.Model.Dynamic;
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

    [Fact]
    public void ObjectCannotBeAddedToSameCollectionTwice()
    {
        var sut = new World();
        var anObject = new DynamicGameObjectSpy();

        sut.AddDynamicObject(anObject);
        Assert.Throws<ArgumentException>(() => sut.AddDynamicObject(anObject));
    }
}