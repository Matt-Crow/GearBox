using GearBox.Core.Model.Dynamic;
using Xunit;

namespace GearBox.Core.Tests.Model.Dynamic;

public class DynamicWorldContentTester
{
    [Fact]
    public void ObjectsCanAddOtherObjects()
    {
        var sut = new DynamicWorldContent();
        sut.AddDynamicObject(new DuplicatingDynamicGameObject(sut));
        
        sut.Update();

        Assert.Equal(2, sut.DynamicObjects.Count());
    }

    [Fact]
    public void ObjectCannotBeAddedToSameCollectionTwice()
    {
        var sut = new DynamicWorldContent();
        var anObject = new DynamicGameObjectSpy();

        sut.AddDynamicObject(anObject);
        sut.AddDynamicObject(anObject);
        sut.Update();

        Assert.Single(sut.DynamicObjects);
    }

    [Fact]
    public void Object_AfterTerminating_IsRemoved()
    {
        var sut = new DynamicWorldContent();
        var anObject = new TerminatingDynamicGameObject();

        sut.AddDynamicObject(anObject);
        anObject.IsTerminated = true;
        sut.Update();

        Assert.Empty(sut.DynamicObjects);
    }
}