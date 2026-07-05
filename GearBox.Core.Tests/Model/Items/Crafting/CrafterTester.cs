using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using Xunit;

namespace GearBox.Core.Tests.Model.Items.Crafting;

public class CrafterTester
{
    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var ingredient = AMaterial();
        var part = APart();
        var recipe = ARecipe(ingredient, part);
        var inventory = new Inventory();
        var sut = ACrafter(recipe);

        sut.Craft(recipe.Id, inventory);

        Assert.Null(inventory.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = AMaterial();
        var part = APart();
        var recipe = ARecipe(ingredient, part);
        var inventory = new Inventory();
        var sut = ACrafter(recipe);

        inventory.Materials.Add(ingredient);
        sut.Craft(recipe.Id, inventory);

        Assert.Empty(inventory.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = AMaterial();
        var part = APart();
        var recipe = ARecipe(ingredient, part);
        var inventory = new Inventory();
        var sut = ACrafter(recipe);
        
        inventory.Materials.Add(ingredient);
        sut.Craft(recipe.Id, inventory);

        Assert.NotNull(inventory.GetBySpecifier(ItemSpecifier.ByName(part.Name)));
    }

    [Fact]
    public void Craft_GivenCanCraft_ReturnsCopy()
    {
        var ingredient = AMaterial();
        var part = APart();
        var recipe = ARecipe(ingredient, part);
        var inventory = new Inventory();
        var sut = ACrafter(recipe);

        inventory.Materials.Add(ingredient);
        sut.Craft(recipe.Id, inventory);
        var actual = inventory.GetBySpecifier(ItemSpecifier.ByName(part.Name));
        
        Assert.NotNull(actual);
        Assert.NotEqual(actual.Id, part.Id);
    }

    private static Material AMaterial() => new Material("some material");
    private static Part APart() => new Part("Some part", PartSlotType.ALL.First());
    
    private static CraftingRecipe ARecipe(Material material, Part part)
    {
        var stack = new ItemStack<Material>(material);
        var result = new CraftingRecipe([stack], ItemUnion.OfPart(part));
        return result;
    }

    private static Crafter ACrafter(CraftingRecipe recipe) => new Crafter(CraftingRecipeRepository.Of([recipe]));
}