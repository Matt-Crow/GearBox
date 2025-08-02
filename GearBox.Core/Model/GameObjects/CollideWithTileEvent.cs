using GearBox.Core.Model.Areas;

namespace GearBox.Core.Model.GameObjects;

public class CollideWithTileEvent
{
    public CollideWithTileEvent(Tile tile) => Tile = tile;

    public Tile Tile { get; init; }
}