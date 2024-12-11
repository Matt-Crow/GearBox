using GearBox.Core.Model.Json;

namespace GearBox.Core.Model.Items;

public interface IItem
{
    /// <summary>
    /// A unique identifier for this item.
    /// Should be null for stackable items of ones which don't need an identity.
    /// </summary>
    Guid? Id { get; }

    /// <summary>
    /// Identifies the specific item this.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// How high quality or rare this item is.
    /// </summary>
    Grade Grade { get; }

    /// <summary>
    /// The base value this item can be bought for from a shop.
    /// </summary>
    Gold BuyValue { get; }

    /// <summary>
    /// Converts to JSON so it can be sent to the front-end.
    /// </summary>
    ItemJson ToJson(int quantity);

    /*
    // can't do "IItem ToOwned()" https://stackoverflow.com/a/5709191
    //IItem ToOwned();
    */
}