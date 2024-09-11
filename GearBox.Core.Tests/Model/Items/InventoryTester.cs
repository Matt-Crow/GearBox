using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Items.Infrastructure;
using GearBox.Core.Model.Units;
using Xunit;

namespace GearBox.Core.Tests.Model.Items;

public class InventoryTester
{
    [Fact]
    public void GetBySpecifier_FindsByIdFirst()
    {
        var sut = new Inventory();
        var weapon1 = new Equipment<WeaponStats>(new ItemType("foo"), new WeaponStats(AttackRange.MELEE));
        var weapon2 = new Equipment<WeaponStats>(new ItemType("foo"), new WeaponStats(AttackRange.MELEE));
        sut.Weapons.Add(weapon1);
        sut.Weapons.Add(weapon2);

        var actual = sut.GetBySpecifier(new ItemSpecifier(weapon2.Id, weapon1.Type.Name));

        Assert.Equal(weapon2, actual?.Weapon);
    }

    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var items = new ItemFactory()
            .Add(ItemUnion.Of(new Material(new ItemType("foo"))))
            .Add(ItemUnion.Of(new Equipment<WeaponStats>(new ItemType("bar"), new WeaponStats())))
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
        var ingredient = new Material(new ItemType("foo"));
        var sut = new Inventory();
        var items = new ItemFactory()
            .Add(ItemUnion.Of(ingredient))
            .Add(ItemUnion.Of(new Equipment<WeaponStats>(new ItemType("bar"), new WeaponStats())))
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
        var ingredient = new Material(new ItemType("foo"));
        var item = new Equipment<WeaponStats>(new ItemType("bar"), new WeaponStats());
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.Of(ingredient))
            .Add(ItemUnion.Of(item))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes("bar");
        
        sut.Craft(recipe);

        Assert.Contains(item.Type, sut.Weapons.Content.Select(stack => stack.Item.Type));
    }
}