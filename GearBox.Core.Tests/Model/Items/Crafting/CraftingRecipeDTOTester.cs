using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CraftingRecipeDTOTester
{
    [Fact]
    public void MakeCraftingRecipe_GivenDuplicate_CombinesStacks()
    {
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfPart(new Part("bar", PartSlotType.ALL.First())))
            ;

        var result = new CraftingRecipeDTO([
            new ItemStackDTO("foo"),
            new ItemStackDTO("foo")
        ], "bar");

        Assert.Single(result.Ingredients);
    }
}