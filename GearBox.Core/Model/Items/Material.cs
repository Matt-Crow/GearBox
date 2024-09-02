namespace GearBox.Core.Model.Items;

/// <summary>
/// A Material is a type of item which are only used for crafting
/// </summary>
public class Material : IItem
{
    public Material(ItemType type, string? description = null)
    {
        Type = type;
        Description = description ?? "no description provided";
        BuyValue = new Gold(type.Grade.BuyValueBase);
    }

    public Guid? Id => null;
    public ItemType Type { get; init; }
    public string Description { get; init; }
    public int Level => 0; // players of any level can use any material
    public IEnumerable<string> Details => []; // materials have no details for now
    public Gold BuyValue { get; init; }

    public override bool Equals(object? obj)
    {
        var other = obj as Material;
        return other != null
            && other.Type.Equals(Type);
    }
    
    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }

    public Material ToOwned()
    {
        return this;
    }
}