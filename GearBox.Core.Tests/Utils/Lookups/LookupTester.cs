using GearBox.Core.Utils.Lookups;
using Xunit;

namespace GearBox.Core.Tests.Utils.Lookups;

public class LookupTester
{
    [Fact]
    public void Of_GivenDuplicateKeys_Throws()
    {
        var values = new List<KeyValue<string>>()
        {
            new KeyValue<string>("foo"),
            new KeyValue<string>("foo")
        };

        Assert.Throws<ArgumentException>(() => Lookup<KeyValue<string>>.Of(values));
    }

    [Fact]
    public void Get_GivenInvalidName_ReturnsNull()
    {
        var sut = Lookup<KeyValue<string>>.Of([]);
        var actual = sut.Get("foo");
        Assert.Null(actual);
    }

    [Fact]
    public void Get_GivenCopier_ReturnsCopy()
    {
        var value = new KeyValue<string>("foo");
        var sut = Lookup<KeyValue<string>>.Of([value], CopyIt);

        var actual = sut.Get(value.Key);

        Assert.NotNull(actual);
        Assert.False(value == actual); // referential equality should fail
    }

    private static KeyValue<string> CopyIt(KeyValue<string> it) => new KeyValue<string>(it.Key);
}