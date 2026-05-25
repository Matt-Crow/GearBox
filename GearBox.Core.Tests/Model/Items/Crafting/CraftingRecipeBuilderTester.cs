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
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfWeapon(new Equipment("bar", EquipmentSlotType.WEAPON)))
            ;
        var sut = new CraftingRecipeBuilder(items);

        var result = sut
            .And("foo")
            .And("foo")
            .Makes("bar");

        Assert.Single(result.Ingredients);
    }
}