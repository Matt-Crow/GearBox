using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives.Impl;

public class Intangible : PassiveAbility
{
    public Intangible() : base("Intangible")
    {
    }

    public override IPassiveAbility Copy() => new Intangible();

    public override string GetDescription() => "Allows the user to walk through walls.";

    protected override void RegisterTo(Character user)
    {
        user.Body.EventCollideWithTile.AddCanceler(IsWall);
    }

    protected override void UnregisterFrom(Character user)
    {
        user.Body.EventCollideWithTile.RemoveCanceler(IsWall);
    }

    private bool IsWall(CollideWithTileEvent e)
    {
        return e.Tile.TileType.Height == TileHeight.WALL;
    }
}