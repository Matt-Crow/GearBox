using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CrafterTester
{
    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var part = APart();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfPart(part))
            ;
        var recipe = items.MakeCraftingRecipe(new CraftingRecipeDTO([new ItemStackDTO("foo")], part.Name));
        var sut = new Crafter(CraftingRecipeRepository.Of([recipe]));
        var inventory = new Inventory();

        sut.Craft(recipe.Id, inventory);

        Assert.Null(inventory.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material("foo");
        var inventory = new Inventory();
        var part = APart();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfPart(part))
            ;
        inventory.Materials.Add(ingredient);
        var recipe = items.MakeCraftingRecipe(new CraftingRecipeDTO([new ItemStackDTO("foo")], part.Name));
        var sut = new Crafter(CraftingRecipeRepository.Of([recipe]));
        
        sut.Craft(recipe.Id, inventory);

        Assert.Empty(inventory.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material("foo");
        var part = APart();
        var inventory = new Inventory();
        inventory.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfPart(part))
            ;
        var recipe = items.MakeCraftingRecipe(new CraftingRecipeDTO([new ItemStackDTO("foo")], part.Name));
        var sut = new Crafter(CraftingRecipeRepository.Of([recipe]));

        sut.Craft(recipe.Id, inventory);

        Assert.NotNull(inventory.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    private Part APart() => new Part("Some part", PartSlotType.ALL.First());
}