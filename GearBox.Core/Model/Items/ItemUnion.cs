using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Implementation of a C union.
/// Contains a single item.
/// </summary>
public class ItemUnion : IItem
{
    private readonly Material? _material;
    private readonly Equipment<WeaponStats>? _weapon;
    private readonly Equipment<ArmorStats>? _armor;


    private ItemUnion(Material? material, Equipment<WeaponStats>? weapon, Equipment<ArmorStats>? armor)
    {
        _material = material;
        _weapon = weapon;
        _armor = armor;
        Unwrapped = (IItem?)_material ?? (IItem?)_weapon ?? (IItem?)_armor ?? throw new ArgumentNullException(null, "One argument must not be null");
    }

    public static ItemUnion Of(Material material) => new ItemUnion(material, null, null);
    public static ItemUnion Of(Equipment<WeaponStats> weapon) => new ItemUnion(null, weapon, null);
    public static ItemUnion Of(Equipment<ArmorStats> armor) => new ItemUnion(null, null, armor);

    /// <summary>
    /// The item contained within this ItemUnion
    /// </summary>
    public IItem Unwrapped { get; init;}

    public Guid? Id => Unwrapped.Id;
    public string Name => Unwrapped.Name;
    public Grade Grade => Unwrapped.Grade;
    public string Description => Unwrapped.Description;
    public int Level => Unwrapped.Level;
    public IEnumerable<string> Details => Unwrapped.Details;
    public Gold BuyValue => Unwrapped.BuyValue;


    public ItemJson ToJson() => Select(
        m => new ItemStack<Material>(m).ToJson(),
        w => new ItemStack<Equipment<WeaponStats>>(w).ToJson(),
        a => new ItemStack<Equipment<ArmorStats>>(a).ToJson()
    );

    /// <summary>
    /// Returns either a copy of the wrapped value or the original wrapped value if it is immutable.
    /// If a level is provided and the wrapped value supports it, the copy will have the provided level.
    /// </summary>
    public ItemUnion ToOwned(int? level=null) => Select(
        m => Of(m.ToOwned()),
        w => Of(w.ToOwned(level)),
        a => Of(a.ToOwned(level))
    );

    /// <summary>
    /// Invokes one of the provided actions on the wrapped value.
    /// The exact action called depends on the type of the wrapped value.
    /// </summary>
    public void Match(Action<Material> withMaterial, Action<Equipment<WeaponStats>> withWeapon, Action<Equipment<ArmorStats>> withArmor)
    {
        if (_material != null)
        {
            withMaterial(_material);
        }
        else if (_weapon != null)
        {
            withWeapon(_weapon);
        }
        else if (_armor != null)
        {
            withArmor(_armor);
        }
        else
        {
            throw new Exception($"Missing case in {nameof(Match)}");
        }
    }

    /// <summary>
    /// Applies one of the provided mapping functions to the wrapped value and returns the result.
    /// The exact mapping function called depends on the type of the wrapped value.
    /// </summary>
    public T Select<T>(Func<Material, T> fromMaterial, Func<Equipment<WeaponStats>, T> fromWeapon, Func<Equipment<ArmorStats>, T> fromArmor)
    {
        if (_material != null)
        {
            return fromMaterial(_material);
        }
        if (_weapon != null)
        {
            return fromWeapon(_weapon);
        }
        if (_armor != null)
        {
            return fromArmor(_armor);
        }
        throw new Exception($"Missing case in {nameof(Select)}");
    }

    public ItemJson ToJson(int quantity) => Unwrapped.ToJson(quantity);
}