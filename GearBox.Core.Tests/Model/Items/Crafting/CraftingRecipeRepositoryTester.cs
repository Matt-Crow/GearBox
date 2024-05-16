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
        var expected = new CraftingRecipe([], () => throw new NotImplementedException());
        var sut = CraftingRecipeRepository.Of([expected]);

        var actual = sut.GetById(expected.Id);

        Assert.Equal(expected, actual);
    }
}