namespace GearBox.Core.Model.Items;

public interface IItem
{
    /// <summary>
    /// A unique identifier for this item.
    /// Should be null for stackable items of ones which don't need an identity.
    /// </summary>
    Guid? Id { get; }

    string Name { get; }

    Grade Grade { get; }

    string Description { get; }

    /// <summary>
    /// The minimum level players must have to use this item
    /// </summary>
    int Level { get; }

    /// <summary>
    /// Details to display in the GUI
    /// </summary>
    IEnumerable<string> Details { get; }

    /// <summary>
    /// The base value this item can be bought for from a shop
    /// </summary>
    Gold BuyValue { get; }

    /*
    // can't do "IItem ToOwned()" https://stackoverflow.com/a/5709191
    //IItem ToOwned();
    */
}