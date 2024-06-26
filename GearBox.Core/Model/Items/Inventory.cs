using GearBox.Core.Model.GameObjects.ChangeTracking;
using GearBox.Core.Model.Items.Crafting;
using GearBox.Core.Model.Json.AreaUpdate;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IMightChange<InventoryJson>
{
    private readonly ChangeTracker<InventoryJson> _changeTracker;

    public Inventory()
    {
        _changeTracker = new(this);
    }

    public InventoryTab<Weapon> Weapons { get; init; } = new();
    public InventoryTab<Armor> Armors { get; init; } = new();
    public InventoryTab<Material> Materials { get; init; } = new();
    public IEnumerable<object?> DynamicValues => Array.Empty<object?>() 
        .Concat(Weapons.DynamicValues)
        .Concat(Armors.DynamicValues)
        .Concat(Materials.DynamicValues);

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
    }

    public bool Any()
    {
        return Weapons.Any() || Armors.Any() || Materials.Any();
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
        var crafted = recipe.Maker.Invoke();
        
        Weapons.Add(crafted.Weapon);
        Armors.Add(crafted.Armor);
        Materials.Add(crafted.Material);
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        var result = recipe.Ingredients.All(ingredient => Materials.Contains(ingredient.Item, ingredient.Quantity));
        return result;
    }

    public void Update() => _changeTracker.Update();
    public MaybeChangeJson<InventoryJson> GetChanges() => _changeTracker.ToJson();
    public InventoryJson ToJson() => new(Weapons.ToJson(), Armors.ToJson(), Materials.ToJson());
}