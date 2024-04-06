using GearBox.Core.Model.Stable.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable.Items;

public class ItemTypeRepositoryTester
{
    [Fact]
    public void Of_GivenDuplicate_Throws()
    {
        var items = new List<ItemType>()
        {
            new ItemType("foo"),
            new ItemType("foo")
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
        var expected = new ItemType("foo");
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
        var type1 = new ItemType("foo");
        var type2 = new ItemType("bar");
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