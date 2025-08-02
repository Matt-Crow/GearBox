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

        sut.EmitEvent(42);

        Assert.False(listenerWasInvoked);
    }

    [Fact]
    public void Cancelers_RunBeforeListeners()
    {
        var listenerWasInvoked = false;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => listenerWasInvoked = true);
        sut.AddCanceler(_ => true);

        sut.EmitEvent(42);

        Assert.False(listenerWasInvoked);
    }

    [Fact]
    public void AddListener_GivenNoSecondArg_DoesNotRemoveAfterInvoking()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++);

        sut.EmitEvent(42);
        sut.EmitEvent(42);

        Assert.Equal(2, count);
    }

    [Fact]
    public void AddListener_GivenSecondArg_RemovesAfterInvokingThatManyTimes()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++, 2);

        sut.EmitEvent(42);
        sut.EmitEvent(42);
        sut.EmitEvent(42);

        Assert.Equal(2, count);
    }

    [Fact]
    public void AddCanceler_GivenSecondArg_RemovesAfterInvokingThatManyTimes()
    {
        var count = 0;
        var sut = new EventEmitter<int>();
        sut.AddListener(_ => count++);
        sut.AddCanceler(_ => true, 2);

        sut.EmitEvent(42);
        sut.EmitEvent(42);
        sut.EmitEvent(42);

        Assert.Equal(1, count);
    }
}