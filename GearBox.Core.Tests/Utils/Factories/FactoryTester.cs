using GearBox.Core.Utils.Factories;
using Xunit;

namespace GearBox.Core.Tests.Utils.Factories;

public class FactoryTester
{
    [Fact]
    public void Of_GivenDuplicateKeys_Throws()
    {
        var values = new List<FactoryProductWrapper<string>>()
        {
            new FactoryProductWrapper<string>("foo"),
            new FactoryProductWrapper<string>("foo")
        };

        Assert.Throws<ArgumentException>(() => Factory<FactoryProductWrapper<string>>.Of(values));
    }

    [Fact]
    public void Get_GivenInvalidName_ReturnsNull()
    {
        var sut = Factory<FactoryProductWrapper<string>>.Of([]);
        var actual = sut.Get("foo");
        Assert.Null(actual);
    }

    [Fact]
    public void Get_GivenCopier_ReturnsCopy()
    {
        var value = new FactoryProductWrapper<string>("foo");
        var sut = Factory<FactoryProductWrapper<string>>.Of([value], CopyIt);

        var actual = sut.Get(value.Key);

        Assert.NotNull(actual);
        Assert.False(value == actual); // referential equality should fail
    }

    private static FactoryProductWrapper<string> CopyIt(FactoryProductWrapper<string> it) => new FactoryProductWrapper<string>(it.Key);
}