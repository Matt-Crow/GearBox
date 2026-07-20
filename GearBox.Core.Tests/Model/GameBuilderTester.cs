using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Areas;
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
            .WithArea(AnArea());
        
        Assert.Throws<ArgumentException>(() => sut.WithArea(AnArea()));
    }

    private static AreaResource AnArea()
    {
        var result = new AreaResource()
        {
            Name = "foo",
            Level = 1,
            Map = new()
            {
                Tiles = [[0]],
                TileTypes = [
                    new TileTypeResource()
                    {
                        Key = 0,
                        ColorName = Color.ALL.First().Name,
                        HeightName = TileHeight.FLOOR.Name
                    }
                ]
            }
        };
        return result;
    }
}