using GearBox.Core.Model.Units;

namespace GearBox.Core.Model.GameObjects;

/// <summary>
/// Raised whenever a game object leaves the bounds of a map
/// </summary>
public class CollideWithMapEdgeEventArgs : EventArgs
{
    public CollideWithMapEdgeEventArgs(Dimensions mapDimensions) => MapDimensions = mapDimensions;

    public Dimensions MapDimensions { get; init; }
}