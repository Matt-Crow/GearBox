using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory 
{
    public Inventory()
    {
        EquipmentTabs = [
            Manipulators,
            Torsos
        ];    
    }


    public InventoryTab<Equipment> Manipulators { get; init; } = new();
    public InventoryTab<Equipment> Torsos { get; init; } = new();
    public InventoryTab<Material> Materials { get; init; } = new();
    public Gold Gold { get; private set; } = Gold.NONE;
    public List<InventoryTab<Equipment>> EquipmentTabs { get; init; }
    public bool IsEmpty => EquipmentTabs.All(tab => tab.IsEmpty) && Materials.IsEmpty && Gold.Quantity == 0;

    /// <summary>
    /// Adds all items from the other inventory to this one
    /// </summary>
    public void Add(Inventory other)
    {
        foreach (var equipmentTab in other.EquipmentTabs)
        {
            // todo no stacks for equipment
            foreach (var equipmentStack in equipmentTab.Content)
            {
                GetTab(equipmentStack.Item).Add(equipmentStack.Item.ToOwned(), equipmentStack.Quantity);
            }
        }
        foreach (var materialStack in other.Materials.Content)
        {
            Materials.Add(materialStack.Item.ToOwned(), materialStack.Quantity);
        }

        Gold = Gold.Plus(other.Gold);
    }

    public void Add(Equipment equipment)
    {
        Add(ItemUnion.OfEquipment(equipment));
    }

    public void Add(ItemUnion? item, int quantity = 1)
    {
        item?.Match(
            m => Materials.Add(m, quantity),
            e => GetTab(e).Add(e, quantity)
        );
    }

    private InventoryTab<Equipment> GetTab(Equipment e) => e.SlotType.GetInventoryTab(this);

    public void Add(Gold? gold)
    {
        if (gold == null)
        {
            return;
        }
        Gold = Gold.Plus(gold);
    }

    public void Remove(ItemUnion? item)
    {
        item?.Match(
            m => Materials.Remove(m),
            e => GetTab(e).Remove(e)
        );
    }

    public void Remove(Gold? gold)
    {
        if (gold == null)
        {
            return;
        }
        Gold = new Gold(Gold.Quantity - gold.Quantity);
    }

    public bool Contains(ItemUnion item)
    {
        var result = item.Select(
            m => Materials.Contains(m),
            e => GetTab(e).Contains(e)
        );
        return result;
    }

    public ItemUnion? GetBySpecifier(ItemSpecifier specifier)
    {
        var equipment = Manipulators.GetBySpecifier(specifier) ?? Torsos.GetBySpecifier(specifier);
        var material = Materials.GetBySpecifier(specifier);
        if (equipment != null)
        {
            return ItemUnion.OfEquipment(equipment);
        }
        if (material != null)
        {
            return ItemUnion.OfMaterial(material);
        }
        return null;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
        {
            return;
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            Materials.Remove(ingredient.Item, ingredient.Quantity);
        }

        /*
            Craft the item at level 1.
            This prevents players from getting overleveled items in low level areas
        */
        var item = recipe.Maker.Invoke();
        Add(item);
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        var result = recipe.Ingredients.All(ingredient => Materials.Contains(ingredient.Item, ingredient.Quantity));
        return result;
    }

    public InventoryJson ToJson() => new(Manipulators.ToJson(), Torsos.ToJson(), Materials.ToJson(), Gold.Quantity);
}