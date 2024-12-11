using GearBox.Core.Model.Areas;

namespace GearBox.Core.Model.GameObjects;

public class CollideWithTileEventArgs : EventArgs
{
    public CollideWithTileEventArgs(Tile tile) => Tile = tile;

    public Tile Tile { get; init; }
}