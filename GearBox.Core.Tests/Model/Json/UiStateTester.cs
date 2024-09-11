using GearBox.Core.Model.Areas;
using GearBox.Core.Model.GameObjects.Player;
using GearBox.Core.Model.Items;
using GearBox.Core.Model.Items.Shops;
using GearBox.Core.Model.Json;
using Xunit;

namespace GearBox.Core.Tests.Model.Json;

public class UiStateTester
{
    [Fact]
    public void OpenShop_AfterPlayerPicksUpItem_Changes()
    {
        var shop = new ItemShop();
        var area = new Area(shops: [shop]);
        var player = new PlayerCharacter("foo");
        player.SetArea(area);
        player.SetOpenShop(shop);
        var before = new UiState(player);

        player.Inventory.Materials.Add(new Material(new ItemType("foo")));
        var after = new UiState(player);
        var diff = before.GetChanges(after);

        Assert.True(diff.OpenShop.HasChanged);
    }
}