using Xunit;

namespace GearBox.Core.Model.Items.Crafting;

public class CraftingRecipeRepositoryTester
{
    [Fact]
    public void GetById_GivenNotFound_ReturnsNull()
    {
        var sut = CraftingRecipeRepository.Empty();
        var result = sut.GetById(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public void GetById_GivenFound_ReturnsIt()
    {
        var expected = new CraftingRecipe([], ItemUnion.OfPart(new Part("foo", PartSlotType.ALL.First())));
        var sut = CraftingRecipeRepository.Of([expected]);

        var actual = sut.GetById(expected.Id);

        Assert.Equal(expected, actual);
    }
}