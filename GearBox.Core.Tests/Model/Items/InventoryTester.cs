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
        var weapon1 = new Equipment("foo");
        var weapon2 = new Equipment("foo");
        sut.Weapons.Add(weapon1);
        sut.Weapons.Add(weapon2);

        var actual = sut.GetBySpecifier(new ItemSpecifier(weapon2.Id, weapon1.Name));

        Assert.Equal(weapon2, actual?.Unwrapped);
    }

    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfWeapon(new Equipment("bar")))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes("bar");

        sut.Craft(recipe);

        Assert.Empty(sut.Weapons.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material("foo");
        var sut = new Inventory();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfWeapon(new Equipment("bar")))
            ;
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes("bar");
        
        sut.Craft(recipe);

        Assert.Empty(sut.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material("foo");
        var item = new Equipment("bar");
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfWeapon(item))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes("bar");
        
        sut.Craft(recipe);

        Assert.Contains(item.Name, sut.Weapons.Content.Select(stack => stack.Item.Name));
    }
}