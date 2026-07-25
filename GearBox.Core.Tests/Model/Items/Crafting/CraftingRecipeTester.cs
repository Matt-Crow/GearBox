using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CraftingRecipeTester
{
    [Fact]
    public void MakeCraftingRecipe_GivenDuplicate_CombinesStacks()
    {
        var result = new CraftingRecipe([
            new ItemStack<Material>(new Material("foo")),
            new ItemStack<Material>(new Material("foo"))
        ], ItemUnion.OfPart(new Part("bar", PartSlotType.ALL.First())));

        Assert.Single(result.Ingredients);
    }
}