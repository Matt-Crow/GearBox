using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory 
{
    public Gold Gold { get; private set; } = Gold.NONE;
    public InventoryTab<Material> Materials { get; init; } = new();
    public List<PartTab> PartTabs { get; init; } = PartSlotType.ALL
        .Select(slotType => new PartTab(slotType))
        .ToList();
    
    public bool IsEmpty => PartTabs.All(tab => tab.IsEmpty) && Materials.IsEmpty && Gold.Quantity == 0;

    /// <summary>
    /// Adds all items from the other inventory to this one
    /// </summary>
    public void Add(Inventory other)
    {
        foreach (var partTab in other.PartTabs)
        {
            // todo no stacks for parts
            foreach (var partStack in partTab.Content)
            {
                GetTab(partStack.Item).Add(partStack.Item.ToOwned(), partStack.Quantity);
            }
        }
        foreach (var materialStack in other.Materials.Content)
        {
            Materials.Add(materialStack.Item.ToOwned(), materialStack.Quantity);
        }

        Gold = Gold.Plus(other.Gold);
    }

    public void Add(Part part)
    {
        Add(ItemUnion.OfPart(part));
    }

    public void Add(ItemUnion? item, int quantity = 1)
    {
        item?.Match(
            m => Materials.Add(m, quantity),
            e => GetTab(e).Add(e, quantity)
        );
    }

    public InventoryTab<Part> GetTab(Part e)
    {
        return PartTabs.First(tab => tab.SlotType == e.SlotType);
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
        var part = PartTabs
            .Select(tab => tab.GetBySpecifier(specifier))
            .FirstOrDefault(e => e != null);
        var material = Materials.GetBySpecifier(specifier);
        if (part != null)
        {
            return ItemUnion.OfPart(part);
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

    public InventoryJson ToJson()
    {
        var partsJson = PartTabs
            .SelectMany(tab => tab.Content)
            .Select(stack => stack.ToJson())
            .ToList();
        return new(partsJson, Materials.ToJson(), Gold.Quantity);
    }
}