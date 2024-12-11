namespace GearBox.Core.Tests.Model.Areas;

using GearBox.Core.Model;
using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects;
using GearBox.Core.Model.GameObjects.Enemies;
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
    public void TilesDefaultToFloor()
    {
        var sut = new Map();
        var aTile = sut.GetTileAt(Coordinates.ORIGIN);
        Assert.Equal(TileHeight.FLOOR, aTile.Height);
    }

    [Fact]
    public void MustAddTileTypeBeforeSettingTiles()
    {
        var sut = new Map();
        Assert.Throws<ArgumentException>(() => sut.SetTileAt(Coordinates.ORIGIN, -1));
    }

    [Fact]
    public void CanSetKeyMultipleTimes()
    {
        var type1 = new TileType(Color.BLUE, TileHeight.WALL);
        var type2 = new TileType(Color.RED, TileHeight.WALL);
        var sut = new Map()
            .SetTileTypeForKey(1, type1)
            .SetTileTypeForKey(1, type2)
            .SetTileAt(Coordinates.ORIGIN, 1);

        var actual = sut.GetTileAt(Coordinates.ORIGIN);

        Assert.Equal(type2, actual);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(100, 0)]
    public void CannotSetTileOutOfBounds(int x, int y)
    {
        var sut = new Map(Dimensions.InTiles(20))
            .SetTileTypeForKey(1, AWall());

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

    [Fact]
    public void TerminateProjectilesOutOfBounds()
    {
        var sut = new Map();
        var projectile = new Projectile(Coordinates.FromTiles(-1, -1), Velocity.ZERO, Distance.FromTiles(5), new Attack(new EnemyCharacter("foo", 1), 1), Color.BLACK);

        sut.CheckForCollisions(projectile.Body);

        Assert.True(projectile.Termination.IsTerminated);
    }

    [Fact]
    public void TerminateProjectilesCollidingWithWalls()
    {
        var sut = new Map()
            .SetTileTypeForKey(1, AWall())
            .SetTileAt(Coordinates.ORIGIN, 1);
        var projectile = new Projectile(Coordinates.ORIGIN.CenteredOnTile(), Velocity.ZERO, Distance.FromTiles(5), new Attack(new EnemyCharacter("foo", 1), 1), Color.BLACK);

        sut.CheckForCollisions(projectile.Body);

        Assert.True(projectile.Termination.IsTerminated);
    }

    [Fact]
    public void CheckForCollisions_GivenInTile_ShovesOut()
    {
        var coordinates = Coordinates.FromTiles(2, 2);
        var sut = new Map()
            .SetTileTypeForKey(1, AWall())
            .SetTileAt(coordinates, 1);
        var inTile = new BodyBehavior()
        {
            Location = coordinates
        };

        sut.CheckForCollisions(inTile);

        Assert.NotEqual(coordinates, inTile.Location);
    }

    private static TileType AWall() => new(Color.RED, TileHeight.WALL);
}