using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class StableWorldContentTester
{
    [Fact]
    public void Add_EmitsContentEvent()
    {
        var sut = new StableWorldContent();
        sut.Add(new StableGameObjectSpy());

        var actual = sut.Update();

        Assert.Contains(actual, change => change.IsContent);
    }

    [Fact]
    public void Add_EmitsContentEventOnlyOnce()
    {
        var sut = new StableWorldContent();
        sut.Add(new StableGameObjectSpy());

        sut.Update();
        var actual = sut.Update();

        Assert.DoesNotContain(actual, change => change.IsContent);
    }

    [Fact]
    public void Update_GivenAnObjectChanges_EmitsContentEvent()
    {
        var sut = new StableWorldContent();
        var spy = new StableGameObjectSpy();
        sut.Add(spy);
        sut.Update(); // clears & applies changes

        spy.Foo++;
        var actual = sut.Update();
        
        Assert.Contains(actual, change => change.IsContent);
    }

    [Fact]
    public void Update_DoesEmitsOnce()
    {
        var sut = new StableWorldContent();
        var spy = new StableGameObjectSpy();
        sut.Add(spy);
        sut.Update(); // clears & applies changes

        spy.Foo++;
        sut.Update();
        var actual = sut.Update();
        
        Assert.DoesNotContain(actual, change => change.IsContent);
    }
}