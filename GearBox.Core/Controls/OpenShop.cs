using GearBox.Core.Model.GameObjects.Player;

namespace GearBox.Core.Controls;

/// <summary>
/// Opens the shop the player is colliding with, if any
/// </summary>
public class OpenShop : IControlCommand
{
    public void ExecuteOn(PlayerCharacter target)
    {
        var area = target.CurrentArea ?? throw new Exception("Cannot open shop when not in an area");
        var openableShop = area.Shops.FirstOrDefault(s => s.CollidesWith(target));
        if (openableShop != null)
        {
            target.SetOpenShop(openableShop);
        }
    }
}
