using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory 
{
    public InventoryTab<Equipment<WeaponStats>> Weapons { get; init; } = new();
    public InventoryTab<Equipment<ArmorStats>> Armors { get; init; } = new();
    public InventoryTab<Material> Materials { get; init; } = new();
    public Gold Gold { get; private set; } = Gold.NONE;
    public bool IsEmpty => Weapons.IsEmpty && Armors.IsEmpty && Materials.IsEmpty && Gold.Quantity == 0;

    /// <summary>
    /// Adds all items from the other inventory to this one
    /// </summary>
    public void Add(Inventory other)
    {
        // todo no stacks for weapons
        foreach (var weaponStack in other.Weapons.Content)
        {
            Weapons.Add(weaponStack.Item.ToOwned(), weaponStack.Quantity);
        }
        // todo no stacks for armors
        foreach (var armorStack in other.Armors.Content)
        {
            Armors.Add(armorStack.Item.ToOwned(), armorStack.Quantity);
        }
        foreach (var materialStack in other.Materials.Content)
        {
            Materials.Add(materialStack.Item.ToOwned(), materialStack.Quantity);
        }

        Gold = Gold.Plus(other.Gold);
    }

    public void Add(ItemUnion? item)
    {
        item?.Match(
            m => Materials.Add(m),
            w => Weapons.Add(w),
            a => Armors.Add(a)
        );
    }

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
            w => Weapons.Remove(w),
            a => Armors.Remove(a)
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
            w => Weapons.Contains(w),
            a => Armors.Contains(a)
        );
        return result;
    }

    public ItemUnion? GetBySpecifier(ItemSpecifier specifier)
    {
        var weapon = Weapons.GetBySpecifier(specifier);
        var armor = Armors.GetBySpecifier(specifier);
        var material = Materials.GetBySpecifier(specifier);
        if (weapon != null)
        {
            return ItemUnion.Of(weapon);
        }
        if (armor != null)
        {
            return ItemUnion.Of(armor);
        }
        if (material != null)
        {
            return ItemUnion.Of(material);
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

    public InventoryJson ToJson() => new(Weapons.ToJson(), Armors.ToJson(), Materials.ToJson(), Gold.Quantity);
}