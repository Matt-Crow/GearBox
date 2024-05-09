namespace GearBox.Core.Model.Items;

/// <summary>
/// Implementation of a C union.
/// Contains a single item.
/// </summary>
public class ItemUnion
{
    private ItemUnion(Material? material, Weapon? weapon)
    {
        Material = material;
        Weapon = weapon;
    }

    public static ItemUnion Of(Material material)
    {
        return new ItemUnion(material, null);
    }

    public static ItemUnion Of(Weapon weapon)
    {
        return new ItemUnion(null, weapon);
    }

    public Material? Material { get; init; }
    public Weapon? Weapon { get; init; }
}