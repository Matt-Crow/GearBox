using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Implementation of a C union.
/// Contains a single item.
/// </summary>
public class ItemUnion
{
    private ItemUnion(Material? material, Weapon? weapon, Armor? armor)
    {
        Material = material;
        Weapon = weapon;
        Armor = armor;
    }

    public static ItemUnion Of(Material material)
    {
        return new ItemUnion(material, null, null);
    }

    public static ItemUnion Of(Weapon weapon)
    {
        return new ItemUnion(null, weapon, null);
    }

    public static ItemUnion Of(Armor armor)
    {
        return new ItemUnion(null, null, armor);
    }

    public Material? Material { get; init; }
    public Weapon? Weapon { get; init; }
    public Armor? Armor { get; init; }

    public ItemJson ToJson()
    {
        if (Material != null)
        {
            return new ItemStack<Material>(Material).ToJson();
        }
        if (Weapon != null)
        {
            return new ItemStack<Weapon>(Weapon).ToJson();
        }
        if (Armor != null)
        {
            return new ItemStack<Armor>(Armor).ToJson();
        }
        throw new Exception("ItemUnion has no item");
    }

    public ItemType GetItemType()
    {
        if (Material != null)
        {
            return Material.Type;
        }
        if (Weapon != null)
        {
            return Weapon.Type;
        }
        if (Armor != null)
        {
            return Armor.Type;
        }
        throw new Exception("Forgot to add clause in ItemUnion.GetItemType");
    }

    public ItemUnion ToOwned()
    {
        if (Material != null)
        {
            return Of(Material.ToOwned());
        }
        if (Weapon != null)
        {
            return Of(Weapon.ToOwned());
        }
        if (Armor != null)
        {
            return Of(Armor.ToOwned());
        }
        throw new Exception("Forgot to add clause in ItemUnion.ToOwned");
    }
}