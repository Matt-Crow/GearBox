using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Model.Areas;

/// <summary>
/// Used when a player enters or leaves an area
/// </summary>
public class AreaChangedEvent
{
    public AreaChangedEvent(PlayerCharacter whoChangedAreas, IArea? oldArea, IArea? newArea)
    {
        WhoChangedAreas = whoChangedAreas;
        OldArea = oldArea;
        NewArea = newArea;
    }

    public PlayerCharacter WhoChangedAreas { get; init; }

    /// <summary>
    /// If null, player is entering an area, but wasn't in an area before (logging in / respawning)
    /// </summary>
    public IArea? OldArea { get; init; }

    /// <summary>
    /// If null, player is leaving an area, but isn't going into a new area (logging out / dying)
    /// </summary>
    public IArea? NewArea { get; init; }
}