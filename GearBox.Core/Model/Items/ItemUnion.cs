using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

/// <summary>
/// Implementation of a C union.
/// Contains a single item.
/// </summary>
public class ItemUnion : IItem
{
    private readonly Material? _material;
    private readonly Part? _part;


    private ItemUnion(Material? material, Part? part)
    {
        _material = material;
        _part = part;
        Unwrapped = (IItem?)_material ?? (IItem?)_part ?? throw new ArgumentNullException(null, "One argument must not be null");
    }


    public static ItemUnion OfMaterial(Material material) => new ItemUnion(material, null);
    public static ItemUnion OfPart(Part part) => new ItemUnion(null, part);


    /// <summary>
    /// The item contained within this ItemUnion
    /// </summary>
    public IItem Unwrapped { get; init;}

    public Guid? Id => Unwrapped.Id;
    public string Name => Unwrapped.Name;
    public Grade Grade => Unwrapped.Grade;
    public Gold BuyValue => Unwrapped.BuyValue;


    public ItemJson ToJson() => Select(
        m => new ItemStack<Material>(m).ToJson(),
        e => new ItemStack<Part>(e).ToJson()
    );

    /// <summary>
    /// Returns either a copy of the wrapped value or the original wrapped value if it is immutable.
    /// If a level is provided and the wrapped value supports it, the copy will have the provided level.
    /// </summary>
    public ItemUnion ToOwned(int? level=null) => Select(
        m => OfMaterial(m.ToOwned()),
        e => OfPart(e.ToOwned(level))
    );

    /// <summary>
    /// Invokes one of the provided actions on the wrapped value.
    /// The exact action called depends on the type of the wrapped value.
    /// </summary>
    public void Match(Action<Material> withMaterial, Action<Part> withPart)
    {
        if (_material != null)
        {
            withMaterial(_material);
        }
        else if (_part != null)
        {
            withPart(_part);
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
    public T Select<T>(Func<Material, T> fromMaterial, Func<Part, T> fromPart)
    {
        if (_material != null)
        {
            return fromMaterial(_material);
        }
        if (_part != null)
        {
            return fromPart(_part);
        }
        throw new Exception($"Missing case in {nameof(Select)}");
    }

    public ItemJson ToJson(int quantity) => Unwrapped.ToJson(quantity);
}