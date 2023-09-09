namespace GearBox.Core.Tests.Model.Static;

using GearBox.Core.Model;
using GearBox.Core.Model.Dynamic;
using GearBox.Core.Model.Static;
using GearBox.Core.Model.Units;
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
        var aTileType = TileType.Tangible(Color.GREEN);
        Assert.Throws<ArgumentException>(() => sut.SetTileAt(Coordinates.ORIGIN, aTileType));
    }

    [Fact]
    public void CanSetKeyMultipleTimes()
    {
        var sut = new Map();
        var type1 = TileType.Tangible(Color.GREEN);
        var type2 = TileType.Tangible(Color.RED);
        
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
        var aTileType = TileType.Tangible(Color.GREEN);
        sut.SetTileTypeForKey(1, aTileType);

        Assert.Throws<ArgumentException>(() => sut.SetTileAt(Coordinates.FromTiles(x, y), 1));
    }

    [Fact]
    public void CheckForCollisions_GivenOutOfBounds_MovesInBounds()
    {
        var sut = new Map();
        var outOfBounds = new BodyBehavior()
        {
            Location = Coordinates.FromPixels(-1, 1000)
        };

        sut.CheckForCollisions(outOfBounds);

        Assert.Equal(Coordinates.FromPixels(outOfBounds.Radius.InPixels, 1000), outOfBounds.Location);
    }
}