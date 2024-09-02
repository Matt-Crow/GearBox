using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Implementation of a C union.
/// Contains a single item.
/// </summary>
public class ItemUnion
{
    private ItemUnion(Material? material, Equipment<WeaponStats>? weapon, Equipment<ArmorStats>? armor)
    {
        Material = material;
        Weapon = weapon;
        Armor = armor;
    }

    public static ItemUnion Of(Material material)
    {
        return new ItemUnion(material, null, null);
    }

    public static ItemUnion Of(Equipment<WeaponStats> weapon)
    {
        return new ItemUnion(null, weapon, null);
    }

    public static ItemUnion Of(Equipment<ArmorStats> armor)
    {
        return new ItemUnion(null, null, armor);
    }

    public Material? Material { get; init; }
    public Equipment<WeaponStats>? Weapon { get; init; }
    public Equipment<ArmorStats>? Armor { get; init; }

    public ItemJson ToJson()
    {
        if (Material != null)
        {
            return new ItemStack<Material>(Material).ToJson();
        }
        if (Weapon != null)
        {
            return new ItemStack<Equipment<WeaponStats>>(Weapon).ToJson();
        }
        if (Armor != null)
        {
            return new ItemStack<Equipment<ArmorStats>>(Armor).ToJson();
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

    public ItemUnion ToOwned(int? level=null)
    {
        if (Material != null)
        {
            return Of(Material.ToOwned());
        }
        if (Weapon != null)
        {
            return Of(Weapon.ToOwned(level));
        }
        if (Armor != null)
        {
            return Of(Armor.ToOwned(level));
        }
        throw new Exception("Forgot to add clause in ItemUnion.ToOwned");
    }

    public Gold BuyValue()
    {
        if (Material != null)
        {
            return Material.BuyValue;
        }
        if (Weapon != null)
        {
            return Weapon.BuyValue;
        }
        if (Armor != null)
        {
            return Armor.BuyValue;
        }
        throw new Exception($"Missing case in {nameof(BuyValue)}");
    }
}