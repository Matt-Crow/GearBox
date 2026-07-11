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

        Assert.Throws<ArgumentException>(() => Factory<FactoryProductWrapper<string>>.Of(It, values));
    }

    [Fact]
    public void Make_GivenInvalidName_Throws()
    {
        var sut = Factory<FactoryProductWrapper<string>>.Of(It, []);
        Assert.Throws<ArgumentException>(() => sut.Make("foo"));
    }

    [Fact]
    public void Make_GivenCopier_ReturnsCopy()
    {
        var value = new FactoryProductWrapper<string>("foo");
        var sut = Factory<FactoryProductWrapper<string>>.Of(CopyIt, [value]);

        var actual = sut.Make(value.Key);

        Assert.False(value == actual); // referential equality should fail
    }

    private static FactoryProductWrapper<string> It(FactoryProductWrapper<string> it) => it;
    private static FactoryProductWrapper<string> CopyIt(FactoryProductWrapper<string> it) => new FactoryProductWrapper<string>(it.Key);
}