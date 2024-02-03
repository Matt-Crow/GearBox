using GearBox.Core.Model.Stable;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable;

public class InventoryItemTypeRepositoryTester
{
    [Fact]
    public void Of_GivenDuplicate_Throws()
    {
        var items = new List<InventoryItemType>()
        {
            InventoryItemType.Stackable("foo"),
            InventoryItemType.NonStackable("foo")
        };

        Assert.Throws<ArgumentException>(() => InventoryItemTypeRepository.Of(items));
    }

    [Fact]
    public void GetByName_GivenNotExists_ReturnsNull()
    {
        var items = Enumerable.Empty<InventoryItemType>();
        var sut = InventoryItemTypeRepository.Of(items);

        var actual = sut.GetByName("foo");

        Assert.Null(actual);
    }

    [Fact]
    public void GetByName_GivenExists_ReturnsIt()
    {
        var expected = InventoryItemType.Stackable("foo");
        var sut = InventoryItemTypeRepository.Of(new List<InventoryItemType>()
        {
            expected
        });

        var actual = sut.GetByName("foo");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAll_Works()
    {
        var type1 = InventoryItemType.Stackable("foo");
        var type2 = InventoryItemType.NonStackable("bar");
        var sut = InventoryItemTypeRepository.Of(new List<InventoryItemType>()
        {
            type1,
            type2
        });

        var actual = sut.GetAll();

        Assert.Contains(type1, actual);
        Assert.Contains(type2, actual);
    }
}