using GearBox.Core.Model.GameObjects;
using Xunit;

namespace GearBox.Core.Tests.Model.GameObjects;

public class GameObjectCollectionTester
{
    [Fact]
    public void ObjectsCanAddOtherObjects()
    {
        var sut = new GameObjectCollection<DuplicatingGameObject>();
        sut.AddGameObject(new DuplicatingGameObject(sut));
        
        sut.Update();

        Assert.Equal(2, sut.AsEnumerable.Count());
    }

    [Fact]
    public void ObjectCannotBeAddedToSameCollectionTwice()
    {
        var sut = new GameObjectCollection<GameObjectSpy>();
        var anObject = new GameObjectSpy();

        sut.AddGameObject(anObject);
        sut.AddGameObject(anObject);
        sut.Update();

        Assert.Single(sut.AsEnumerable);
    }

    [Fact]
    public void Object_AfterTerminating_IsRemoved()
    {
        var sut = new GameObjectCollection<TerminatingGameObject>();
        var anObject = new TerminatingGameObject();

        sut.AddGameObject(anObject);
        anObject.IsTerminated = true;
        sut.Update();

        Assert.Empty(sut.AsEnumerable);
    }
}