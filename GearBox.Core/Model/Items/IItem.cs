namespace GearBox.Core.Model.Items;

public interface IItem
{
    /// <summary>
    /// A unique identifier for this item.
    /// Should be null for stackable items of ones which don't need an identity.
    /// </summary>
    Guid? Id { get; }

    /// <summary>
    /// Items of the same type can stack together in a player's inventory
    /// </summary>
    ItemType Type { get; }

    string Description { get; }

    /// <summary>
    /// The minimum level players must have to use this item
    /// </summary>
    int Level { get; }

    /// <summary>
    /// Details to display in the GUI
    /// </summary>
    IEnumerable<string> Details { get; }

    /*
    // can't do "IItem ToOwned()" https://stackoverflow.com/a/5709191
    //IItem ToOwned();
    */
}