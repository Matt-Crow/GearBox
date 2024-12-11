using GearBox.Core.Config;
using GearBox.Core.Model;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class GameBuilderTester
{
    [Fact]
    public void AreaNameMustBeUnique()
    {
        var sut = new GameBuilder(new GearBoxConfig())
            .WithArea("foo", 1, area => area);
        
        Assert.Throws<ArgumentException>(() => sut.WithArea("foo", 1, area => area));
    }
}