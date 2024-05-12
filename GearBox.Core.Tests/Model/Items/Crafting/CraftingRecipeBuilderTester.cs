using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CraftingRecipeBuilderTester
{
    [Fact]
    public void And_GivenDuplicate_CombinesStacks()
    {
        var ingredient = new Material(new ItemType("foo"));
        var sut = new CraftingRecipeBuilder();

        var result = sut
            .And(ingredient)
            .And(ingredient)
            .Makes(() => ItemUnion.Of(new Weapon(new ItemType("bar"))));

        Assert.Single(result.Ingredients);
    }
}