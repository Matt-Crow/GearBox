using GearBox.Core.Model.Items;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class InventoryTester
{
    [Fact]
    public void GetBySpecifier_FindsByIdFirst()
    {
        var sut = new Inventory();
        var part1 = APart();
        var part2 = APart();
        sut.Add(part1);
        sut.Add(part2);

        var actual = sut.GetBySpecifier(new ItemSpecifier(part2.Id, part1.Name));

        Assert.Equal(part2, actual?.Unwrapped);
    }

    private Part APart() => new Part("Some part", PartSlotType.ALL.First());
}