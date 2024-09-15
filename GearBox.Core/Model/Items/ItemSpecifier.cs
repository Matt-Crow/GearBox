namespace GearBox.Core.Model.Items;

/// <summary>
/// Allows locating an item by either ID or name
/// </summary>
public class ItemSpecifier
{
    public ItemSpecifier(Guid? id, string? name)
    {
        Id = id;
        Name = name;
    }

    public static ItemSpecifier ById(Guid id) => new ItemSpecifier(id, null);
    
    public Guid? Id { get; init; }
    public string? Name { get; init; }

    public bool Matches(IItem item)
    {
        var idMatches = Id == null || item.Id == Id;
        var nameMatches = Name == null || item.Name == Name;
        return idMatches && nameMatches;
    }
}