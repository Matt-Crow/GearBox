using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class ItemTypeRepositoryTester
{
    [Fact]
    public void Of_GivenDuplicate_Throws()
    {
        var items = new List<ItemType>()
        {
            ItemType.Stackable("foo"),
            ItemType.NonStackable("foo")
        };

        Assert.Throws<ArgumentException>(() => ItemTypeRepository.Of(items));
    }

    [Fact]
    public void GetByName_GivenNotExists_ReturnsNull()
    {
        var items = Enumerable.Empty<ItemType>();
        var sut = ItemTypeRepository.Of(items);

        var actual = sut.GetByName("foo");

        Assert.Null(actual);
    }

    [Fact]
    public void GetByName_GivenExists_ReturnsIt()
    {
        var expected = ItemType.Stackable("foo");
        var sut = ItemTypeRepository.Of(new List<ItemType>()
        {
            expected
        });

        var actual = sut.GetByName("foo");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAll_Works()
    {
        var type1 = ItemType.Stackable("foo");
        var type2 = ItemType.NonStackable("bar");
        var sut = ItemTypeRepository.Of(new List<ItemType>()
        {
            type1,
            type2
        });

        var actual = sut.GetAll();

        Assert.Contains(type1, actual);
        Assert.Contains(type2, actual);
    }
}