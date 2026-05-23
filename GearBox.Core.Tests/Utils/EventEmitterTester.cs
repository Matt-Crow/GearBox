using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Utils;

public class EventEmitterTester
{
    [Fact]
    public void Modifiers_RunBeforeCancelers()
    {
        var listenerWasInvoked = false;
        var sut = new EventEmitter<int>();
        sut.AddModifier(e => e - 1);
        sut.AddCanceler(e => e == 41);
        sut.AddListener(_ => listenerWasInvoked = true);

        sut.ProcessEvent(42, _ => {});

        Assert.False(listenerWasInvoked);
    }

    [Fact]
    public void Action_GivenCanceled_DoesNotRun()
    {
        var actionWasInvoked = false;
        var sut = new EventEmitter<int>();
        sut.AddCanceler(_ => true);

        sut.ProcessEvent(42, _ => actionWasInvoked = true);

        Assert.False(actionWasInvoked);
    }

    [Fact]
    public void Cancelers_RunBeforeListeners()
    {
        var listenerWasInvoked = false;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => listenerWasInvoked = true);
        sut.AddCanceler(_ => true);

        sut.ProcessEvent(42, _ => {});

        Assert.False(listenerWasInvoked);
    }

    [Fact]
    public void Listeners_RunAfterAction()
    {
        var result = new List<string>();
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => result.Add("Listener"));

        sut.ProcessEvent(42, _ => result.Add("Action"));

        Assert.Equal(2, result.Count);
        Assert.Equal("Action", result[0]);
        Assert.Equal("Listener", result[1]);
    }

    [Fact]
    public void Action_ReceivesModifiedEvent()
    {
        var actual = 0;
        var sut = new EventEmitter<int>();
        sut.AddModifier(i => i * 2);

        sut.ProcessEvent(4, e => actual = e);

        Assert.Equal(8, actual);
    }

    [Fact]
    public void AddListener_GivenNoSecondArg_DoesNotRemoveAfterInvoking()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++);

        sut.ProcessEvent(42, _ => {});
        sut.ProcessEvent(42, _ => {});

        Assert.Equal(2, count);
    }

    [Fact]
    public void AddListener_GivenSecondArg_RemovesAfterInvokingThatManyTimes()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++, 2);

        sut.ProcessEvent(42, _ => {});
        sut.ProcessEvent(42, _ => {});
        sut.ProcessEvent(42, _ => {});

        Assert.Equal(2, count);
    }

    [Fact]
    public void AddCanceler_GivenSecondArg_RemovesAfterInvokingThatManyTimes()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++);
        sut.AddCanceler(_ => true, 2);

        sut.ProcessEvent(42, _ => {});
        sut.ProcessEvent(42, _ => {});
        sut.ProcessEvent(42, _ => {});

        Assert.Equal(1, count);
    }

    [Fact]
    public void RemoveListener_RemovesBeforeEmittingNextEvent()
    {
        var triggered = false;
        Action<int> listener = _ => triggered = true;
        var sut = new EventEmitter<int>();
        sut.AddListener(listener);

        sut.RemoveListener(listener);
        sut.ProcessEvent(42, _ => {});

        Assert.False(triggered);
    }
}