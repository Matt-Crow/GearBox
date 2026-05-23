using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// Raised whenever a game object leaves the bounds of a map
/// </summary>
public class CollideWithMapEdgeEvent
{
    public CollideWithMapEdgeEvent(Dimensions mapDimensions) => MapDimensions = mapDimensions;

    public Dimensions MapDimensions { get; init; }
}