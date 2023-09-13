using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class StableWorldContentTester
{
    [Fact]
    public void Add_EmitsCreateEvent()
    {
        var sut = new StableWorldContent();
        sut.Add(new StableGameObjectSpy());

        var actual = sut.Update();

        Assert.Contains(actual, change => change.IsCreate);
    }

    [Fact]
    public void ClearPendingChanges_RemovesFromUpdateResult()
    {
        var sut = new StableWorldContent();
        sut.Add(new StableGameObjectSpy());

        sut.ClearPendingChanges();
        var actual = sut.Update();

        Assert.Empty(actual);
    }

    [Fact]
    public void Update_GivenAnObjectChanges_EmitsUpdateEvent()
    {
        var sut = new StableWorldContent();
        var spy = new StableGameObjectSpy();
        sut.Add(spy);
        sut.ClearPendingChanges();

        spy.Foo++;
        var actual = sut.Update();

        Assert.Contains(actual, change => change.IsUpdate);
    }
}