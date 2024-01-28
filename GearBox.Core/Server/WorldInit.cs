using GearBox.Core.Model;
using GearBox.Core.Model.Stable;
using GearBox.Core.Model.Static;

namespace GearBox.Core.Server;

/// <summary>
/// Sent as a message to users upon first connecting to a world.
/// It contains all information which will never change, and thus only needs to
/// be sent once.
/// </summary>
public class WorldInit : IJson
{
    public WorldInit(Guid playerId, StaticWorldContentJson staticWorldContent, List<InventoryItemTypeJson> itemTypes)
    {
        PlayerId = playerId;
        StaticWorldContent = staticWorldContent;
        ItemTypes = itemTypes;
    }

    /// <summary>
    /// The Id of the character spawned into the world for the user.
    /// </summary>
    public Guid PlayerId { get; init; }

    /// <summary>
    /// The contents of the world which never change.
    /// </summary>
    public StaticWorldContentJson StaticWorldContent { get; init; }

    /// <summary>
    /// All the possible item types which can exist in the world.
    /// </summary>
    public List<InventoryItemTypeJson> ItemTypes { get; init; }
}