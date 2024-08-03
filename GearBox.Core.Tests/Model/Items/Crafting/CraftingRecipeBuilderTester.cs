using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CraftingRecipeBuilderTester
{
    [Fact]
    public void And_GivenDuplicate_CombinesStacks()
    {
        var items = new ItemFactory()
            .Add(ItemUnion.Of(new Material(new ItemType("foo"))))
            .Add(ItemUnion.Of(new Weapon(new ItemType("bar"))))
            ;
        var sut = new CraftingRecipeBuilder(items);

        var result = sut
            .And("foo")
            .And("foo")
            .Makes("bar");

        Assert.Single(result.Ingredients);
    }
}