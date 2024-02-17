using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Utils;

public class SafeListTester
{
    [Fact]
    public void Add_WhileIterating_Works()
    {
        var sut = new SafeList<int>(1, 2, 3);

        foreach (var item in sut.AsEnumerable())
        {
            sut.Add(item * 2);
        }

        Assert.Equal(3, sut.Count);
    }

    [Fact]
    public void Add_DoesNotRunUntilAfterApplyChanges()
    {
        var sut = new SafeList<int>();
        sut.Add(1);

        Assert.Empty(sut.AsEnumerable());
        sut.ApplyChanges();
        Assert.Single(sut.AsEnumerable());
    }
}