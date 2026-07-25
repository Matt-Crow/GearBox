using GearBox.Core.Config;
using GearBox.Core.Model;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.ResourcePacks;
using GearBox.Core.Utils;
using Xunit;

namespace GearBox.Core.Tests.Model;

public class GameResourcesTester
{
    [Fact]
    public void AreaNameMustBeUnique()
    {
        var sut = new GameResources()
        {
            ResourcePacks = [
                new ResourcePack()
                {
                    Areas = [
                        AnArea(),
                        AnArea()
                    ]
                }
            ]
        };
        Assert.Throws<Exception>(() => sut.ToGame(
            new GearBoxConfig(), 
            new RandomNumberGenerator()
        ));
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