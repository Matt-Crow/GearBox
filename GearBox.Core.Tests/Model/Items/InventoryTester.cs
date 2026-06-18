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
        var equipment1 = AnEquipment();
        var equipment2 = AnEquipment();
        sut.Add(equipment1);
        sut.Add(equipment2);

        var actual = sut.GetBySpecifier(new ItemSpecifier(equipment2.Id, equipment1.Name));

        Assert.Equal(equipment2, actual?.Unwrapped);
    }

    [Fact]
    public void Craft_GivenCannotCraft_DoesNothing()
    {
        var sut = new Inventory();
        var equipment = AnEquipment();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(new Material("foo")))
            .Add(ItemUnion.OfEquipment(equipment))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(equipment.Name);

        sut.Craft(recipe);

        Assert.Null(sut.GetBySpecifier(ItemSpecifier.ByName(equipment.Name)));
    }

    [Fact]
    public void Craft_GivenCanCraft_RemovesIngredients()
    {
        var ingredient = new Material("foo");
        var sut = new Inventory();
        var equipment = AnEquipment();
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfEquipment(equipment))
            ;
        sut.Materials.Add(ingredient);
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(equipment.Name);
        
        sut.Craft(recipe);

        Assert.Empty(sut.Materials.Content);
    }

    [Fact]
    public void Craft_GivenCanCraft_AddsItem()
    {
        var ingredient = new Material("foo");
        var equipment = AnEquipment();
        var sut = new Inventory();
        sut.Materials.Add(ingredient);
        var items = new ItemFactory()
            .Add(ItemUnion.OfMaterial(ingredient))
            .Add(ItemUnion.OfEquipment(equipment))
            ;
        var recipe = new CraftingRecipeBuilder(items)
            .And("foo")
            .Makes(equipment.Name);
        
        sut.Craft(recipe);

        Assert.NotNull(sut.GetBySpecifier(ItemSpecifier.ByName(equipment.Name)));
    }

    private Equipment AnEquipment() => new Equipment("Some equipment", EquipmentSlotType.WEAPON);
}