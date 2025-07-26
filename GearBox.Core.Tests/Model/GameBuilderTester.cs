using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class GameBuilderTester
{
    [Fact]
    public void AreaNameMustBeUnique()
    {
        var sut = new GameBuilder(new GearBoxConfig(), new RandomNumberGenerator())
            .WithArea("foo", 1, area => area);
        
        Assert.Throws<ArgumentException>(() => sut.WithArea("foo", 1, area => area));
    }
}