namespace GearBox.Core.Model.Items;

/// <summary>
/// A Material is a type of item which are only used for crafting
/// </summary>
public class Material : IItem
{
    public Material(string name, Grade? grade = null, string? description = null)
    {
        Name = name;
        Grade = grade ?? Grade.COMMON;
        Description = description ?? "no description provided";
        BuyValue = new Gold(Grade.BuyValueBase);
    }

    public Guid? Id => null;
    public string Name { get; init; }
    public Grade Grade { get; init; }
    public string Description { get; init; }
    public int Level => 0; // players of any level can use any material
    public IEnumerable<string> Details => []; // materials have no details for now
    public Gold BuyValue { get; init; }

    public override bool Equals(object? obj)
    {
        return obj is Material other && other.Name.Equals(Name);
    }
    
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public Material ToOwned()
    {
        return this;
    }
}