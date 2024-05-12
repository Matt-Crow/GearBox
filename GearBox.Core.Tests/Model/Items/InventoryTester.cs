using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class InventoryTester
{
    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var recipe = new CraftingRecipeBuilder()
            .And(new(new Material(new ItemType("foo"))))
            .Makes(() => ItemUnion.Of(new Weapon(new ItemType("bar"))));

        sut.Craft(recipe);

        Assert.Empty(sut.Weapons.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material(new ItemType("foo"));
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder()
            .And(new(ingredient))
            .Makes(() => ItemUnion.Of(new Weapon(new ItemType("bar"))));
        
        sut.Craft(recipe);

        Assert.Empty(sut.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material(new ItemType("foo"));
        var item = new Weapon(new ItemType("bar"));
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder()
            .And(new(ingredient))
            .Makes(() => ItemUnion.Of(item));
        
        sut.Craft(recipe);

        Assert.Contains(item, sut.Weapons.Content.Select(stack => stack.Item));
    }
}