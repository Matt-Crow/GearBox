using GearBox.Core.Model.Dynamic.Player;
using GearBox.Core.Model.Stable.Items;
using GearBox.Core.Model.Units;
using Xunit;

namespace GearBox.Core.Tests.Model.Stable.Items;

public class LootChestTester
{
    [Fact]
    public void CheckForCollisions_GivenPlayerCollides_AddsToInventory()
    {
        var item = new Material(new ItemType("foo"));
        var sut = new LootChest(Coordinates.ORIGIN, item);
        var player = new PlayerCharacter
        {
            Coordinates = sut.Body.Location
        };

        sut.CheckForCollisions(player);

        Assert.Contains(item, player.Inventory.Materials.Content.Select(stack => stack.Item));
    }
}