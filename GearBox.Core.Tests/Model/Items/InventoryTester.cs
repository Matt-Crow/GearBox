using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
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

    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var part = APart();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfPart(part))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(part.Name);

        sut.Craft(recipe);

        Assert.Null(sut.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material("foo");
        var sut = new Inventory();
        var part = APart();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfPart(part))
            ;
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(part.Name);
        
        sut.Craft(recipe);

        Assert.Empty(sut.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material("foo");
        var part = APart();
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfPart(part))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(part.Name);
        
        sut.Craft(recipe);

        Assert.NotNull(sut.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    private Part APart() => new Part("Some part", PartSlotType.ALL.First());
}