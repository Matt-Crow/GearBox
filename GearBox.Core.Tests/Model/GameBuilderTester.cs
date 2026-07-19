using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class GameBuilderTester
{
    [Fact]
    public void AreaNameMustBeUnique()
    {
        var sut = new GameBuilder(new GearBoxConfig(), new RandomNumberGenerator(), new GameResources())
            .WithArea(new AreaResource()
            {
                Name = "foo",
                Level = 1
            }, area => area);
        
        Assert.Throws<ArgumentException>(() => sut.WithArea(new AreaResource()
        {
            Name = "foo",
            Level = 1
        }, area => area));
    }
}