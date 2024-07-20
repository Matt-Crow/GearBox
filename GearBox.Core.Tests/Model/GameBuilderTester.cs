using GearBox.Core.Model;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class GameBuilderTester
{
    [Fact]
    public void AreaNameMustBeUnique()
    {
        var sut = new GameBuilder()
            .WithArea("foo", area => area);
        
        Assert.Throws<ArgumentException>(() => sut.WithArea("foo", area => area));
    }
}