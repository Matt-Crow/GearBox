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
        var weapon1 = AWeapon();
        var weapon2 = AWeapon();
        sut.Weapons.Add(weapon1);
        sut.Weapons.Add(weapon2);

        var actual = sut.GetBySpecifier(new ItemSpecifier(weapon2.Id, weapon1.Name));

        Assert.Equal(weapon2, actual?.Unwrapped);
    }

    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var weapon = AWeapon();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfWeapon(weapon))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(weapon.Name);

        sut.Craft(recipe);

        Assert.Empty(sut.Weapons.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material("foo");
        var sut = new Inventory();
        var weapon = AWeapon();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfWeapon(weapon))
            ;
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(weapon.Name);
        
        sut.Craft(recipe);

        Assert.Empty(sut.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material("foo");
        var weapon = AWeapon();
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfWeapon(weapon))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(weapon.Name);
        
        sut.Craft(recipe);

        Assert.Contains(weapon.Name, sut.Weapons.Content.Select(stack => stack.Item.Name));
    }

    private Equipment AWeapon() => new Equipment("weapon", EquipmentSlotType.WEAPON);
}