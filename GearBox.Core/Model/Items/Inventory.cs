using GearBox.Core.Model.Json;
using GearBox.Core.Model.GameObjects.ChangeTracking;
using System.Text.Json;
using GearBox.Core.Model.Items.Crafting;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Contains items a player has picked up.
/// </summary>
public class Inventory : IDynamic
{
    private readonly ChangeTracker _changeTracker;
    private bool _updatedLastFrame = true;

    public Inventory()
    {
        Serializer = new("inventory", Serialize);
        _changeTracker = new(this);
    }

    public Serializer Serializer { get; init; }
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

    public void Update()
    {
        _updatedLastFrame = _changeTracker.HasChanged;
        _changeTracker.Update();
    }

    private string Serialize(SerializationOptions options)
    {
        var json = new InventoryJson(Weapons.ToJson(), Armors.ToJson(), Materials.ToJson());
        return JsonSerializer.Serialize(json, options.JsonSerializerOptions);
    }

    public StableJson ToJson()
    {
        var result = _updatedLastFrame
            ? StableJson.Changed(Serializer.Serialize().Content)
            : StableJson.NoChanges();
        return result;
    }
}