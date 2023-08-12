namespace GearBox.Core.Tests.Model.Static;

using GearBox.Core.Model;
using GearBox.Core.Model.Static;
using System.Drawing;
using Xunit;

public class MapTester
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void SizeInTilesMustBePositive(int sizeInTiles)
    {
        Assert.Throws<ArgumentException>(() => new Map(Dimensions.InTiles(sizeInTiles)));
    }

    [Fact]
    public void TilesDefaultToIntangible()
    {
        var sut = new Map();
        var aTile = sut.GetTileAt(Coordinates.ORIGIN);
        Assert.False(aTile.IsTangible);
    }

    [Fact]
    public void MustAddTileTypeBeforeSettingTiles()
    {
        var sut = new Map();
        var aTileType = TileType.Tangible(Color.Green);
        Assert.Throws<ArgumentException>(() => sut.SetTileAt(Coordinates.ORIGIN, aTileType));
    }

    [Fact]
    public void CanSetKeyMultipleTimes()
    {
        var sut = new Map();
        var type1 = TileType.Tangible(Color.Green);
        var type2 = TileType.Tangible(Color.Red);
        
        sut.SetTileTypeForKey(1, type1);
        sut.SetTileTypeForKey(1, type2);
        sut.SetTileAt(Coordinates.ORIGIN, 1);
        var actual = sut.GetTileAt(Coordinates.ORIGIN);

        Assert.Equal(type2, actual);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(100, 0)]
    public void CannotSetTileOutOfBounds(int x, int y)
    {
        var sut = new Map(Dimensions.InTiles(20));
        var aTileType = TileType.Tangible(Color.Green);
        sut.SetTileTypeForKey(1, aTileType);

        Assert.Throws<ArgumentException>(() => sut.SetTileAt(Coordinates.InTiles(x, y), 1));
    }
}