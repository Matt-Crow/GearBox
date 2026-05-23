using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects;

namespace GearBox.Core.Model.Abilities.Passives.Impl;

public class Levitate : PassiveAbility
{
    public Levitate() : base("Levitate")
    {
    }

    public override IPassiveAbility Copy() => new Levitate();

    public override string GetDescription() => "Allows the user to move over pits.";

    protected override void RegisterTo(Character user)
    {
        user.Body.EventCollideWithTile.AddCanceler(IsPit);
    }

    protected override void UnregisterFrom(Character user)
    {
        user.Body.EventCollideWithTile.RemoveCanceler(IsPit);
    }

    private bool IsPit(CollideWithTileEvent e)
    {
        return e.Tile.TileType.Height == TileHeight.PIT;
    }
}